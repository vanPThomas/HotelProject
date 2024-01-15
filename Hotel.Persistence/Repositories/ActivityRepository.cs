using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Hotel.Domain.Interfaces;
using Hotel.Domain.Model;

namespace Hotel.Persistence.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly string _connectionString;

        public ActivityRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IReadOnlyList<Activity> GetActivities(string filter)
        {
            List<Activity> activities = new List<Activity>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Activity WHERE Description LIKE @Filter";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Filter", $"%{filter}%");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Activity activity = MapActivityFromReader(reader);
                            activities.Add(activity);
                        }
                    }
                }
            }

            return activities;
        }

        public void AddActivity(Activity activity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Insert Tarif
                    string tarifQuery =
                        "INSERT INTO Tarif (adultTarif, childTarif, discount, adultAge) "
                        + "VALUES (@AdultTarif, @ChildTarif, @Discount, @AdultAge); SELECT SCOPE_IDENTITY();";

                    int tarifId;
                    using (
                        SqlCommand tarifCommand = new SqlCommand(
                            tarifQuery,
                            connection,
                            transaction
                        )
                    )
                    {
                        tarifCommand.Parameters.AddWithValue(
                            "@AdultTarif",
                            activity.Tarif.AdultTarif
                        );
                        tarifCommand.Parameters.AddWithValue(
                            "@ChildTarif",
                            activity.Tarif.ChildTarif
                        );
                        tarifCommand.Parameters.AddWithValue("@Discount", activity.Tarif.Discount);
                        tarifCommand.Parameters.AddWithValue("@AdultAge", activity.Tarif.AdultAge);

                        tarifId = Convert.ToInt32(tarifCommand.ExecuteScalar());
                    }

                    // Insert Description
                    string descriptionQuery =
                        "INSERT INTO Description (name, location, duration, description) "
                        + "VALUES (@Name, @Location, @Duration, @DescriptionText); SELECT SCOPE_IDENTITY();";

                    int descriptionId;
                    using (
                        SqlCommand descriptionCommand = new SqlCommand(
                            descriptionQuery,
                            connection,
                            transaction
                        )
                    )
                    {
                        descriptionCommand.Parameters.AddWithValue(
                            "@Name",
                            activity.Description.Name
                        );
                        descriptionCommand.Parameters.AddWithValue(
                            "@Location",
                            activity.Description.Location
                        );
                        descriptionCommand.Parameters.AddWithValue(
                            "@Duration",
                            activity.Description.Duration
                        );
                        descriptionCommand.Parameters.AddWithValue(
                            "@DescriptionText",
                            activity.Description.TotalDescription
                        );

                        descriptionId = Convert.ToInt32(descriptionCommand.ExecuteScalar());
                    }

                    // Insert Activity
                    string activityQuery =
                        "INSERT INTO Activity (date, numberOfPlaces, tarifId, descriptionId) "
                        + "VALUES (@Date, @NumberOfPlaces, @TarifId, @DescriptionId)";

                    using (
                        SqlCommand command = new SqlCommand(activityQuery, connection, transaction)
                    )
                    {
                        command.Parameters.AddWithValue("@Date", activity.Date);
                        command.Parameters.AddWithValue("@NumberOfPlaces", activity.NumberOfPlaces);
                        command.Parameters.AddWithValue("@TarifId", tarifId);
                        command.Parameters.AddWithValue("@DescriptionId", descriptionId);

                        command.ExecuteNonQuery();
                    }

                    // Commit the transaction if all operations are successful
                    transaction.Commit();
                }
                catch (Exception)
                {
                    // An error occurred, rollback the transaction
                    transaction.Rollback();
                    throw; // Rethrow the exception to notify the caller about the error
                }
            }
        }

        public void UpdateActivity(Activity activity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query =
                    "UPDATE Activity SET Date = @Date, NumberOfPlaces = @NumberOfPlaces, "
                    + "TarifId = @TarifId, DescriptionId = @DescriptionId WHERE Id = @ActivityId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Date", activity.Date);
                    command.Parameters.AddWithValue("@NumberOfPlaces", activity.NumberOfPlaces);
                    command.Parameters.AddWithValue("@TarifId", activity.Tarif.Id);
                    command.Parameters.AddWithValue("@DescriptionId", activity.Description.Id);
                    command.Parameters.AddWithValue("@ActivityId", activity.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteActivity(int activityId)
        {
            // Implement soft-delete logic or additional checks as needed
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "DELETE FROM Activity WHERE Id = @ActivityId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ActivityId", activityId);

                    command.ExecuteNonQuery();
                }
            }
        }

        private Activity MapActivityFromReader(SqlDataReader reader)
        {
            int id = reader.GetInt32(reader.GetOrdinal("Id"));
            DateTime date = reader.GetDateTime(reader.GetOrdinal("Date"));
            int numberOfPlaces = reader.GetInt32(reader.GetOrdinal("NumberOfPlaces"));
            int tarifId = reader.GetInt32(reader.GetOrdinal("TarifId"));
            int descriptionId = reader.GetInt32(reader.GetOrdinal("DescriptionId"));

            // You may need to fetch Tarif and Description information separately depending on your schema

            Activity activity = new Activity
            {
                Id = id,
                Date = date,
                NumberOfPlaces = numberOfPlaces,
                // Populate other properties as needed
            };

            return activity;
        }
    }
}

namespace Inrstructure.PostgreSQL
{
    public class PostgresSettings
    {
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 5655;
        public string User { get; set; } = "postgres";
        public string Password { get; set; } = "password";
        public string Database { get; set; } = "office_db";

        public string ToConnectionString()
        {
            if (string.IsNullOrWhiteSpace(Host))
            {
                string error = "PostgresSettings: Host не задан.";
                throw new InvalidOperationException(error);
            }
            if (Port < 1 || Port > 65535)
            {
                string error = "PostgresSettins: Port вне допустимого диапазона";
                throw new InvalidOperationException(error);
            }
            if (string.IsNullOrWhiteSpace(User))
            {
                string error = "PostgresSettings: User не задан.";
                throw new InvalidOperationException(error);
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                string error = "PostgresSettings: Password не задан.";
                throw new InvalidOperationException(error);
            }
            if (string.IsNullOrWhiteSpace(Database))
            {
                string error = "PostgresSettings: Database не задан.";
                throw new InvalidOperationException(error);
            }
            string format = "Host={0};Port={1};Username={2};Password={3};Database={4};";
            return string.Format(format, Host, Port, User, Password, Database);
        }
    }
}

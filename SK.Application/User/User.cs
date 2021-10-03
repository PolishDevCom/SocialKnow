namespace SK.Application.User
{
    public class User
    {
        /// <summary>
        /// User username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Authentication token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// User main photo
        /// </summary>
        public string Image { get; set; }
    }
}
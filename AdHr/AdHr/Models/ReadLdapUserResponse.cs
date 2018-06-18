namespace AdHr.Models
{
    public class ReadLdapUserResponse
    {
        public string UserCn { get; set; }
        public string UserSn { get; set; }
        public string UserDn { get; set; }
        public string Description { get; internal set; }
    }
}

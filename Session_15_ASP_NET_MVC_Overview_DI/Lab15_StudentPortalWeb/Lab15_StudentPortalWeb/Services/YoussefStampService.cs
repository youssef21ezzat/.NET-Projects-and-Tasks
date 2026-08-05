namespace Lab15_StudentPortalWeb.Services
{
    public class YoussefStampService : IYoussefStampService
    {
        public string Owner => "Youssef Ezzat";
        public string Stamp { get; }
        public YoussefStampService()
        {
            Stamp = Guid.NewGuid().ToString().Substring(0, 8);
        }
    }

}

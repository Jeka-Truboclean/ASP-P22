namespace ASP_P22.Services.Random
{
    public class GuidRandom : IRandomService
    {
        public string FileName()
        {
            return Guid.NewGuid().ToString();
        }
    }
}

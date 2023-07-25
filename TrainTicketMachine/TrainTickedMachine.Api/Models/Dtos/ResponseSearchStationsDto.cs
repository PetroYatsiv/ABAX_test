namespace TrainTickedMachine.Api.Models.Dtos
{
    public class ResponseSearchStationsDto
    {
        public IEnumerable<string>? Stations { get; set; }
        public IEnumerable<char>? NextLetters { get; set; }
    }
}

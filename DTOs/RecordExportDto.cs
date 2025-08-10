namespace VirocGanpati.DTOs
{
    public class RecordExportDto
    {
        public string TenementNumber { get; set; }
        public int WardNo { get; set; }
        public string PropertyType { get; set; }
        public string OccupancyType { get; set; }
        public string OwnerName { get; set; }
        public string OwnerSurveyName { get; set; }
        public string OccupierName { get; set; }
        public string OccupierSurveyName { get; set; }
        public string PropertyAddress { get; set; }
        public string AddressSurvey { get; set; }
        public string MobileNumber { get; set; }
        public string PropertyDescription { get; set; }
        public bool IsGovernmentProperty { get; set; }
        public int YearOfConstruction { get; set; }
        public int NumberOfGutterConnections { get; set; }
        public int NumberOfWaterConnections { get; set; }
        public double OpenArea { get; set; }
        public double GroundFloorArea { get; set; }
        public double FirstFloorArea { get; set; }
        public double SecondFloorArea { get; set; }
        public double ThirdFloorArea { get; set; }
        public double FourthFloorArea { get; set; }
        public double FifthFloorArea { get; set; }
        public string PropertySituation { get; set; }
    }
}

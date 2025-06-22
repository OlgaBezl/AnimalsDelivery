namespace Scripts.Cars.Model
{
    public class CarModel
    {
        private int _animalsCount;

        public CarModel(int id, int colorIndex, int seatsCount)
        {
            Id = id;
            _animalsCount = 0;
            ColorIndex = colorIndex;
            SeatsCount = seatsCount;
            Status = CarModelStatus.FirstParkingGray;
        }

        public int Id { get; private set; }
        public int ColorIndex { get; private set; }
        public int SeatsCount { get; private set; }
        public int OrderParking { get; private set; }
        public CarModelStatus Status { get; private set; }
        public bool IsLinked => SeatsCount == _animalsCount;

        public void AddAnimal()
        {
            _animalsCount++;
        }

        public void ChangeStatus(CarModelStatus status)
        {
            Status = status;
        }

        public void MoveToSecondParking(int orderParking)
        {
            OrderParking = orderParking;
            ChangeStatus(CarModelStatus.SecondParkingMove);
        }
    }
}
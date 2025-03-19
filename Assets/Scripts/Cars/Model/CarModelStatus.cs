namespace Scripts.Cars.Model
{
    public enum CarModelStatus
    {
        FirstParkingGray = 1,
        FirstParkingColor = 2,
        FirstParkingLast = 3,
        QueueWait = 4,
        SecondParkingMove = 5,
        SecondParkingStay = 6,
        SecondParkingLeft = -1
    }
}
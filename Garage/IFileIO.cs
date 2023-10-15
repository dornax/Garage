namespace Garage
{
    internal interface IFileIO
    {
        public IGarageHandler ReadFromFile(IGarageHandler handler, IUtil iUtil, string fileName);
        public void WriteToFile(IGarageHandler handler, IUtil iUtil, string fileName);
    }
}
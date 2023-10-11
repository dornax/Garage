namespace Garage
{
    internal interface IFileIO
    {
        void ReadFromFile(IGarageHandler handler, string fileName);
        void WriteToFile(IGarageHandler handler, string fileName);
    }
}
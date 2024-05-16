public class BlockClass
{
    private string blockName;
    private string fileName;
    public BlockClass(string blockName, string fileName)
    {
        this.blockName = blockName;
        this.fileName = fileName;
    }
    public string BlockName
    { get { return blockName; } set { blockName = value; } }
    public string FileName
    {
        get { return fileName; }
        set { fileName = value; }
    }
}
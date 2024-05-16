using Autodesk.AutoCAD.DatabaseServices;
using System;

public class WorkingDatabase : IDisposable
{
    Database previous;

    public WorkingDatabase(Database newWorkingDb)
    {
        if (newWorkingDb == null) throw new ArgumentNullException(nameof(newWorkingDb));

        Database current = HostApplicationServices.WorkingDatabase;
        if (newWorkingDb != current)
        {
            previous = current;
            HostApplicationServices.WorkingDatabase = newWorkingDb;
        }
    }

    public void Dispose()
    {
        if (previous != null)
        {
            HostApplicationServices.WorkingDatabase.Dispose();
            previous = null;
        }
    }
}
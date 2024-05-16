using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

public class Assistant
{
   public static void MergePDF(List<string> files, string merge)
    {
        try
        {
            var doc = new Document();
            PdfCopy writer = new PdfCopy(doc, new FileStream(merge, FileMode.Create));

            doc.Open();
            PdfReader reader;
            foreach(var f in files)
            {
                reader  = new PdfReader(f);
                for(int i = 1; i <= reader.NumberOfPages; i++)
                {
                    writer.AddPage(writer.GetImportedPage(reader, i));
                }
            }
            writer.Close();
            doc.Close();
        }
        catch(Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}
using Paroxe.PdfRenderer;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class GuideUI : UIAlertLayer
{
    public PDFViewer PDF; 
    protected override void Start()
    {
        base.Start();
        setMinSize();
    }
    public void Refresh()
    {
        PDF.ReloadDocument();
    }


}

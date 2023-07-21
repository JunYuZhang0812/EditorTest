using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.IO;
using Common;
using UnityEngine.UI;

public class SerializeTest
{
    [MenuItem("Window/序列化/写入")]
    public static void WriteBin()
    {
        string binPath = "Assets/StreamingAssets/Bin/SerializeTest.bin";
        string imgPath = "Assets/Resources/TestImg.png";
        if (File.Exists(binPath))
        {
            File.Delete(binPath);
        }
        using (Stream img = File.Open(imgPath, FileMode.Open))
        {
            using (var re = new BinaryReader(img))
            {
                byte[] imgBuffer = new byte[re.BaseStream.Length];
                re.Read(imgBuffer, 0, Convert.ToInt32(re.BaseStream.Length));
                var bw = new BinaryWriter(new FileStream(binPath, FileMode.Create));
                BinarySerializer.Write(bw, imgBuffer);
                bw.Close();
                AssetDatabase.Refresh();
            }
        }
    }
    [MenuItem("Window/序列化/读取")]
    public static void ReadBin()
    {
        string binPath = "Assets/StreamingAssets/Bin/SerializeTest.bin";
        string imgPath = "Assets/Resources/TestImg(Clone).png";
        using (Stream configfs = File.Open(binPath, FileMode.Open))
        {
            using (var re = new BinaryReader(configfs))
            {
                var imgBuffer = BinarySerializer.Read<byte[]>(re);
                var bw = new BinaryWriter(new FileStream(imgPath, FileMode.Create));
                bw.Write(imgBuffer);
                bw.Close();
                AssetDatabase.Refresh();
            }
        }
        Sprite s = Resources.Load<Sprite>("TestImg(Clone)");
        var img = GameObject.Find("Image").GetComponent<Image>();
        img.sprite = s;
    }
}


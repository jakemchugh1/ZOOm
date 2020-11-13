using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class TileData 
{
    Direction exitDir;
    Direction entryDir;

 
    public TileData (Direction en, Direction ex) {
        this.exitDir = ex;
        this.entryDir = en;
    }
 
    public Direction exitDirection
    {
        get
        {
            return this.exitDir;
        }
 
        set
        {
            this.exitDir = value;
        }
    } 
 
    public Direction entryDirection
    {
        get
        {
            return this.entryDir;
        }
        set
        {
            this.entryDir = value;
        }
    } 
}

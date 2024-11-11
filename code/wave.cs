using System;
using System.Collections.Generic;

public class wave
{
    public List<litWave> litWaves = new List<litWave>();
    public wave()
    {
        for (int i = 0; i < EditUi.Instance.lWaveN.ItemCount; i++)
        {
            litWaves.Add(new litWave());
        }
    }
}
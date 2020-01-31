using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FindPath
{
  
    
    public enum Status
    {
        visited, nonVisited
    }


    public static List<Hex> FindPathInHex(Hex start, Hex target, Dictionary<string, Hex> range)
    {
        List<Hex> path = new List<Hex>();
        Queue<Hex> kolejka = new Queue<Hex>();
        bool sukces = false;
        Dictionary<Hex, Hex> visited = new Dictionary<Hex, Hex>();

        Hex curent;
        //foreach (var item in range)
        //{
        //  Debug.Log("Wszystkie w zasiegu : " +item.Value);

        //}
        if (Hex.EqualHex(start, target) == true)
        {
            return path;
        }
        if (range.ContainsKey(target.ToString()))
        {
            kolejka.Enqueue(start);
            visited.Add(start, start);
            //Debug.Log("start");
            while (kolejka.Count > 0)
            {
                curent = kolejka.Dequeue();

                for (int i = 0; i < 6; i++)
                {
                    Hex temp = curent.Add(Hex.directionsArray[i]);
                    //Debug.Log((range.ContainsKey(temp.ToString())) + "Dupa");
                    if (visited.ContainsKey(temp) == false)
                    {
                        if (range.ContainsKey(temp.ToString()) == true)
                        {
                            //Debug.Log("szukam");
                            if (!Hex.EqualHex(temp, target))
                            {
                                visited.Add(temp, temp);
                                temp.parentInPath = curent;
                                kolejka.Enqueue(temp);



                            }
                            else
                            {
                                //Debug.Log("udało sie");
                                temp.parentInPath = curent;
                                if (temp.parentInPath.ToString() != start.ToString())
                                {
                                    path.Add(temp);
                                    while (temp.parentInPath.ToString() != start.ToString())
                                    {

                                        temp = temp.parentInPath;
                                        path.Add(temp);
                                    }
                                }
                                else
                                {

                                    path.Add(temp);
                                }
                                return path;
                            }
                        }
                    }

                }


            }
        }

        
        return path;
    }
        
}

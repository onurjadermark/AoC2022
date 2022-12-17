﻿using System.Text;

namespace Solutions.Solutions._2022;

public class Day16
{
    public int Part1(string[] input)
    {
        var valves = input.Select(ParseValve).ToList();
        ParseValveConnections(input, valves);

        var valvesWithPressure = valves.Where(x => x.Pressure > 0).ToList();
        var distances = new Dictionary<(Valve a, Valve b), int>();
        foreach (var a in valves)
        foreach (var b in valves)
        {
            distances[(a, b)] = int.MaxValue;
        }

        foreach (var curValve in valves)
        {
            var seen = new HashSet<Valve>();
            var queue = new Queue<(Valve Valve, int Distance)>();
            queue.Enqueue((curValve, 0));
            seen.Add(curValve);
            distances[(curValve, curValve)] = 0;
            while (queue.Any())
            {
                var cur = queue.Dequeue();
                foreach (var connection in cur.Valve.Connections)
                {
                    if (seen.Contains(connection)) continue;
                    seen.Add(connection);
                    if (distances[(curValve, connection)] > cur.Distance)
                        distances[(curValve, connection)] = cur.Distance + 1;
                    queue.Enqueue((connection, cur.Distance + 1));
                }
            }
        }

        var maxReleasedPressure = 0;
        var memo = new Dictionary<string, int>();
        var stack = new Stack<(HashSet<Valve> OpenedValves, Valve Location, int Pressure, int Time)>();
        stack.Push((new HashSet<Valve>(), valves.Single(x => x.Name == "AA"), 0, 0));

        while (stack.Any())
        {
            var cur = stack.Pop();
            var location = cur.Location;
            var releasedPressure = cur.Pressure;
            var openedValves = cur.OpenedValves.ToHashSet();
            var time = cur.Time;
            if (HasAlreadySeenBetter(memo, GetState(openedValves, location, 0, location, 0, time),
                    releasedPressure)) continue;

            foreach (var valve in valvesWithPressure.Except(openedValves))
            {
                time = cur.Time;
                if (time > 30) continue;
                location = cur.Location;
                releasedPressure = cur.Pressure;
                openedValves = cur.OpenedValves.ToHashSet();

                if (maxReleasedPressure < releasedPressure) maxReleasedPressure = releasedPressure;
                if (time == 30) continue;

                var distance = distances[(location, valve)];
                for (var i = 0; i < distance; i++)
                {
                    releasedPressure += openedValves.Sum(x => x.Pressure);
                    time++;
                    if (time == 30) break;
                }

                if (maxReleasedPressure < releasedPressure) maxReleasedPressure = releasedPressure;
                if (time == 30) continue;

                releasedPressure += openedValves.Sum(x => x.Pressure);
                openedValves.Add(valve);
                time++;
                if (maxReleasedPressure < releasedPressure) maxReleasedPressure = releasedPressure;
                if (time == 30) continue;

                if (openedValves.Count == valvesWithPressure.Count)
                {
                    for (; time < 30; time++)
                    {
                        releasedPressure += openedValves.Sum(x => x.Pressure);
                    }
                }

                if (maxReleasedPressure < releasedPressure) maxReleasedPressure = releasedPressure;
                if (time == 30) continue;

                stack.Push((openedValves, valve, releasedPressure, time));
            }
        }

        return maxReleasedPressure;
    }

    private static bool HasAlreadySeenBetter(Dictionary<string, int> memo, string state, int releasedPressure)
    {
        if (memo.ContainsKey(state) && memo[state] > releasedPressure) return true;
        memo[state] = releasedPressure;
        return false;
    }
    
    private static string GetState(HashSet<Valve> openedValves, Valve myLocation, int myDistance,
        Valve elephantLocation, int elephantDistance, int time)
    {
        var sortedValves = openedValves.OrderBy(x => x.Name);
        var sb = new StringBuilder();
        foreach (var valve in sortedValves)
        {
            sb.Append(valve.Name);
            sb.Append("-");
        }

        // if (String.Compare(myLocation.Name, elephantLocation.Name, StringComparison.Ordinal) < -1)
        // {
        //     sb.Append(myDistance);
        //     sb.Append(myLocation.Name);
        //     sb.Append(elephantDistance);
        //     sb.Append(elephantLocation.Name);
        // }
        // else
        // {
        //     sb.Append(elephantDistance);
        //     sb.Append(elephantLocation.Name);
        //     sb.Append(myDistance);
        //     sb.Append(myLocation.Name);
        //     
        // }
        

        sb.Append(time);
    
        return sb.ToString();
    }

    private void ParseValveConnections(string[] input, List<Valve> valves)
    {
        foreach (var line in input)
        {
            var curValve = valves.Single(x => x.Name == line.Split(" ")[1]);
            var connectionNamesStr = line.Split(line.Split("valves").Length == 2 ? "valves" : "valve")[1].Trim();
            var connectionNames = connectionNamesStr.Split(",").Select(x => x.Trim()).ToList();
            foreach (var connectionName in connectionNames)
            {
                curValve.Connections.Add(valves.Single(x => x.Name == connectionName));
            }
        }
    }

    private Valve ParseValve(string line, int index)
    {
        return new Valve(line.Split(" ")[1], int.Parse(line.Split(" ")[4].Split("=")[1].Trim(';')));
    }

    public int Part2(string[] input)
    {
        var valves = input.Select(ParseValve).ToList();
        ParseValveConnections(input, valves);

        var pressureValves = valves.Where(x => x.Pressure > 0).ToList();
        var distances = new Dictionary<(Valve a, Valve b), int>();
        foreach (var a in valves)
        foreach (var b in valves)
        {
            distances[(a, b)] = int.MaxValue;
        }

        foreach (var curValve in valves)
        {
            var seen = new HashSet<Valve>();
            var queue = new Queue<(Valve Valve, int Distance)>();
            queue.Enqueue((curValve, 0));
            seen.Add(curValve);
            distances[(curValve, curValve)] = 0;
            while (queue.Any())
            {
                var cur = queue.Dequeue();
                foreach (var connection in cur.Valve.Connections)
                {
                    if (seen.Contains(connection)) continue;
                    seen.Add(connection);
                    if (distances[(curValve, connection)] > cur.Distance)
                        distances[(curValve, connection)] = cur.Distance + 1;
                    queue.Enqueue((connection, cur.Distance + 1));
                }
            }
        }

        var maxReleasedPressure = 0;
        var memo = new Dictionary<string, int>();
        var pq = new PriorityQueue<(HashSet<Valve> OpenedValves, Valve MyValve, int MyDistance,
            Valve ElephantValve, int ElephantDistance, int ReleasedPressure, int ElapsedTime), int>();
        var startValve = valves.Single(x => x.Name == "AA");
        pq.Enqueue((new HashSet<Valve>(), startValve, 0, startValve, 0, 0, 0), 0);

        while (pq.Count > 0)
        {
            var cur = pq.Dequeue();
            var openedValves = cur.OpenedValves;
            var myValve = cur.MyValve;
            var myDistance = cur.MyDistance;
            var elephantValve = cur.ElephantValve;
            var elephantDistance = cur.ElephantDistance;
            var releasedPressure = cur.ReleasedPressure;
            var time = cur.ElapsedTime;
            
            // if (HasAlreadySeenBetter(memo,
            //         GetState(openedValves, myValve, myDistance, elephantValve, elephantDistance, time),
            //         releasedPressure)) continue;
            
            releasedPressure += openedValves.Sum(x => x.Pressure);
            if (maxReleasedPressure < releasedPressure)
            {
                maxReleasedPressure = releasedPressure;
                Console.WriteLine(maxReleasedPressure);
            }

            time++;
            if (time > 26) continue;

            if (myDistance == 0 && elephantDistance == 0)
            {
                if (myValve != startValve) openedValves.Add(myValve);
                if (elephantValve != startValve) openedValves.Add(elephantValve);
                if (pressureValves.Count == openedValves.Count)
                {
                    pq.Enqueue((openedValves.ToHashSet(), myValve, 0, elephantValve, 0, releasedPressure, time),
                        releasedPressure);
                }

                for (int i = 0; i < pressureValves.Count; i++)
                {
                    for (int j = 0; j < pressureValves.Count; j++)
                    {
                        var myNextValve = pressureValves[i];
                        var nextElephantValve = pressureValves[j];
                        if (openedValves.Contains(myNextValve) || openedValves.Contains(nextElephantValve) || myNextValve == nextElephantValve) continue;
                        pq.Enqueue((openedValves.ToHashSet(), myNextValve, distances[(myValve, myNextValve)],
                                nextElephantValve, distances[(elephantValve, nextElephantValve)], releasedPressure,
                                time),
                            releasedPressure);
                    }
                }
            }
            else if (myDistance == 0 && elephantDistance > 0)
            {
                openedValves.Add(myValve);
                if (pressureValves.Count == openedValves.Count)
                {
                    pq.Enqueue((openedValves.ToHashSet(), myValve, 0, elephantValve, 0, releasedPressure, time),
                        releasedPressure);
                }
                foreach (var myNextValve in pressureValves.Except(openedValves))
                {
                    pq.Enqueue((openedValves.ToHashSet(), myNextValve, distances[(myValve, myNextValve)], 
                        elephantValve, elephantDistance - 1, releasedPressure, time), releasedPressure);
                }
            }
            else if (myDistance > 0 && elephantDistance == 0)
            {
                openedValves.Add(elephantValve);
                if (pressureValves.Count == openedValves.Count)
                {
                    pq.Enqueue((openedValves.ToHashSet(), myValve, 0, elephantValve, 0, releasedPressure, time),
                        releasedPressure);
                }
                foreach (var nextElephantValve in pressureValves.Except(openedValves).Except(new[] {myValve}).Reverse())
                {
                    pq.Enqueue((openedValves.ToHashSet(), myValve, myDistance - 1, 
                        nextElephantValve, distances[(elephantValve, nextElephantValve)], releasedPressure, time), releasedPressure);
                }
            }
            else if (myDistance > 0 && elephantDistance > 0)
            {
                pq.Enqueue((openedValves.ToHashSet(), myValve, myDistance - 1,
                    elephantValve, elephantDistance - 1, releasedPressure, time), releasedPressure);
            }
        }

        return maxReleasedPressure;
    }

    private class Valve
    {
        public Valve(string name, int pressure)
        {
            Name = name;
            Pressure = pressure;
        }

        public string Name { get; }
        public int Pressure { get; }
        public HashSet<Valve> Connections { get; } = new();
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialIntelligenceGraphChallenge
{
    internal class Graph
    {
        // Sākotnējais stāvoklis
        public Status startStatus;
        // Saraksts, kurš glāba visus stāvokļūs, kuri tika izveidoti (lai nebūtu dažādi mērķa stāvokli).
        public List<Status> listOfCreatedStatuses = new List<Status>();
   
        public Graph() 
        {
            CreateStartStatus();
            StartSpanning();
        }

        // Sākuma stāvokļa izveide.
        public void CreateStartStatus()
        {
            Adventurer_A adv_A = new Adventurer_A();
            Adventurer_B adv_B = new Adventurer_B();
            Adventurer_C adv_C = new Adventurer_C();
            List<Adventurer> adventurers = new List<Adventurer>
            {
                adv_A,
                adv_B,
                adv_C
            };

            Status start = new Status();
            start.adventurersWaiting = adventurers;
            start.SetTimeSpent(12);

            this.startStatus = start;

            listOfCreatedStatuses.Add(startStatus);
        }

        // Visu iespējamo gadījumu izveide.
        public void StartSpanning()
        {
            // 1. cikls - sadalījums pa objektiem, kas paliek P1 (citi divi objekti iet uz P2).
            for(int i = startStatus.adventurersWaiting.Count - 1; i >= 0; i--)
            {
                Status firstLayerStatus = new Status(startStatus);

                Adventurer firstAdventurerToMove = firstLayerStatus.adventurersWaiting.ElementAt(i);

                Adventurer secondAdventurerToMove;

                if (i <= 0)
                    secondAdventurerToMove = firstLayerStatus.adventurersWaiting.ElementAt(firstLayerStatus.adventurersWaiting.Count - 1);
                else
                    secondAdventurerToMove = firstLayerStatus.adventurersWaiting.ElementAt(i-1);

                firstLayerStatus.adventurersCrossed.Add(firstAdventurerToMove);
                firstLayerStatus.adventurersCrossed.Add(secondAdventurerToMove);
                firstLayerStatus.adventurersWaiting.Remove(firstAdventurerToMove);
                firstLayerStatus.adventurersWaiting.Remove(secondAdventurerToMove);

                firstLayerStatus.SetTimeSpent(firstLayerStatus.GetTimeSpent() - GetTimeUponMoving(firstAdventurerToMove, secondAdventurerToMove));

                AddStatusToTheList(startStatus, firstLayerStatus);

                // 2. cikls - viens no objektiem iet atpakaļ no P2 uz P1, lai paņemtu pēdējo.

                for (int j = 0; j < firstLayerStatus.adventurersCrossed.Count; j++)
                {
                    Status secondLayerStatus = new Status(firstLayerStatus);

                    Adventurer adventurerToGoBack = secondLayerStatus.adventurersCrossed.ElementAt(j);
                    secondLayerStatus.adventurersWaiting.Add(adventurerToGoBack);
                    secondLayerStatus.adventurersCrossed.Remove(adventurerToGoBack);

                    secondLayerStatus.SetTimeSpent(secondLayerStatus.GetTimeSpent() - GetTimeUponMoving(adventurerToGoBack));

                    AddStatusToTheList(firstLayerStatus, secondLayerStatus);

                    // 3. - pārējie elementi ej no P1 uz P2.

                    Status thirdLayerStatus = new Status(secondLayerStatus);

                    firstAdventurerToMove = thirdLayerStatus.adventurersWaiting.ElementAt(0);
                    secondAdventurerToMove = thirdLayerStatus.adventurersWaiting.ElementAt(1); // Error, because decrements twice

                    thirdLayerStatus.adventurersCrossed.Add(firstAdventurerToMove);
                    thirdLayerStatus.adventurersCrossed.Add(secondAdventurerToMove);
                    thirdLayerStatus.adventurersWaiting.Remove(firstAdventurerToMove);
                    thirdLayerStatus.adventurersWaiting.Remove(secondAdventurerToMove);

                    thirdLayerStatus.SetTimeSpent(thirdLayerStatus.GetTimeSpent() - GetTimeUponMoving(firstAdventurerToMove, secondAdventurerToMove));

                    AddStatusToTheList(secondLayerStatus, thirdLayerStatus);
                }
            }
        }

        /// <summary>
        /// Method to get the time to cross the bridge, if two people are crossing the bridge (returns the longest).
        /// </summary>
        /// <param name="adv1">First adventurer to cross.</param>
        /// <param name="adv2">Second adventurer to cross</param>
        /// <returns></returns>
        public int GetTimeUponMoving(Adventurer adv1, Adventurer adv2)
        {
            if (adv1.GetTimeToCross() > adv2.GetTimeToCross())
                return adv1.GetTimeToCross();
            else
                return adv2.GetTimeToCross();
        }

        /// <summary>
        /// Method to get the time to cross the bridge, if one person is crossing the bridge.
        /// </summary>
        /// <param name="adv">Adventurer to cross.</param>
        /// <returns></returns>
        public int GetTimeUponMoving(Adventurer adv)
        {
            return adv.GetTimeToCross();
        }

        /// <summary>
        /// Method to add a new Status to the list of created Statuses.
        /// </summary>
        /// <param name="existingStatus">Status that exists to connect with the new status (if new status already exists).</param>
        /// <param name="statusToAdd">Status that can be added, if list doesn't have it.</param>
        public void AddStatusToTheList(Status existingStatus, Status statusToAdd)
        {
            if(statusToAdd.GetTimeSpent() <= 0)
                return;

            foreach (Status status in listOfCreatedStatuses)
            {
                if (status.GetStatusInfo() == statusToAdd.GetStatusInfo())
                {
                    existingStatus.SetNextStatus(statusToAdd);
                    return;
                }
            }

            listOfCreatedStatuses.Add(statusToAdd);
        }

        /// <summary>
        /// Method that outputs every single Status information.
        /// </summary>
        public void PrintOutAllStatuses()
        {
            Console.WriteLine("Visi stāvoķļi telpas stāvokļu grafā (pēc izveides momenta kārtas):");
            foreach(Status status in listOfCreatedStatuses)
            {
                Console.WriteLine(status.GetStatusInfo());
            }
        }

        /// <summary>
        /// Hardcoded method to print out the resulting graph. Viable for one and only possible graph.
        /// </summary>
        public void PrintOutTheGraph()
        {
            Console.WriteLine("Stāvokļu telpas grafs:");
            Console.WriteLine("\t\t\t\t12;ABC;" +
                "\n\t\t / \t\t | \t\t \\" +
                "\n\t      7;A;BC\t\t9;C;AB\t\t7;B;AC" +
                "\n\t        / \\ \t\t / \\ \t\t / \\" +
                "\n\t  2;AC;B   4;AB;C   6;BC;A   8;AC;B  6;AB;C  2;BC;A" +
                "\n \t\t\t\\  / \t\t \\  /" +
                "\n\t\t\t 1;;ABC \t 3;;ABC");
            Console.WriteLine();
        }
    }
}
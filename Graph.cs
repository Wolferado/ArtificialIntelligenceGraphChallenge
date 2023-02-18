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
        // Lāpas degšanas laiks, kad lāpu var izmantot, lai šķersotu turpu-šurpu tilti.
        private const int maxTimePossible = 12;

        public Graph() 
        {
            CreateStartStatus();
            StartSpanning();
            PrintOutAllStatuses();
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
            start.SetTimeSpent(0);

            this.startStatus = start;

            listOfCreatedStatuses.Add(startStatus);

            Console.WriteLine("Start Status is created.");
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

                firstLayerStatus.SetTimeSpent(firstLayerStatus.GetTimeSpent() + GetTimeUponMoving(firstAdventurerToMove, secondAdventurerToMove));

                AddStatusToTheList(startStatus, firstLayerStatus);

                // 2. cikls - viens no objektiem iet atpakaļ no P2 uz P1, lai paņemtu pēdējo.

                for (int j = 0; j < firstLayerStatus.adventurersCrossed.Count; j++)
                {
                    Status secondLayerStatus = new Status(firstLayerStatus);

                    Adventurer adventurerToGoBack = secondLayerStatus.adventurersCrossed.ElementAt(j);
                    secondLayerStatus.adventurersWaiting.Add(adventurerToGoBack);
                    secondLayerStatus.adventurersCrossed.Remove(adventurerToGoBack);

                    secondLayerStatus.SetTimeSpent(secondLayerStatus.GetTimeSpent() + GetTimeUponMoving(adventurerToGoBack));

                    AddStatusToTheList(firstLayerStatus, secondLayerStatus);

                    // 3. - pārējie elementi ej no P1 uz P2.

                    Status thirdLayerStatus = new Status(secondLayerStatus);

                    firstAdventurerToMove = thirdLayerStatus.adventurersWaiting.ElementAt(0);
                    secondAdventurerToMove = thirdLayerStatus.adventurersWaiting.ElementAt(1); // Error, because decrements twice

                    thirdLayerStatus.adventurersCrossed.Add(firstAdventurerToMove);
                    thirdLayerStatus.adventurersCrossed.Add(secondAdventurerToMove);
                    thirdLayerStatus.adventurersWaiting.Remove(firstAdventurerToMove);
                    thirdLayerStatus.adventurersWaiting.Remove(secondAdventurerToMove);

                    thirdLayerStatus.SetTimeSpent(thirdLayerStatus.GetTimeSpent() + GetTimeUponMoving(firstAdventurerToMove, secondAdventurerToMove));

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
            if(statusToAdd.GetTimeSpent() > maxTimePossible)
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
            foreach(Status status in listOfCreatedStatuses)
            {
                Console.WriteLine(status.GetStatusInfo());
            }
        }
    }
}
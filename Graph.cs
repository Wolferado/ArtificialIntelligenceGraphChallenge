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
        // Sākotnējais stāvoklis grafam.
        public Status startStatus;
        // Saraksts, kurš glāba visus stāvokļus, kuri tika izveidoti.
        public List<Status> listOfCreatedStatuses = new List<Status>();


        /// <summary>
        /// Konstruktors grafam.
        /// </summary>
        public Graph() 
        {
            CreateStartStatus();
            StartSpanning();
        }

        /// <summary>
        /// Metode sākuma stāvokļa izveidei.
        /// </summary>
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
            start.SetTimeLeft(12);


            this.startStatus = start;

            listOfCreatedStatuses.Add(startStatus);
        }

        /// <summary>
        /// Metode visu īespējamo stāvokļu izveidei. Balstīts uz problēmu par ceļotāju un tilta šķērsošanu.
        /// </summary>
        public void StartSpanning()
        {
            // P1 sākotnējais stāvoklis - 3 ceļotāji.
            // P2 sākotnējais stāvoklis - 0 ceļotāji.
            // Laiks škērsošanai no P1 līdz P2 visiem 3 ceļotājiem - 12.

            // 1. cikls - sadalījums pa ceļotājiem, kurš paliek P1 (pārējie 2 ceļotāji iet uz P2).
            for(int i = startStatus.adventurersWaiting.Count - 1; i >= 0; i--)
            {
                Status firstLayerStatus = new Status(startStatus);

                Adventurer firstAdventurerToMove = firstLayerStatus.adventurersWaiting.ElementAt(i);

                Adventurer secondAdventurerToMove;

                if (i <= 0)
                    secondAdventurerToMove = firstLayerStatus.adventurersWaiting.ElementAt(firstLayerStatus.adventurersWaiting.Count - 1);
                else
                    secondAdventurerToMove = firstLayerStatus.adventurersWaiting.ElementAt(i-1);

                MoveAdventurersToP2(firstLayerStatus, firstAdventurerToMove, secondAdventurerToMove);

                firstLayerStatus.SetTimeLeft(firstLayerStatus.GetTimeSpent() - GetTimeUponMoving(firstAdventurerToMove, secondAdventurerToMove));

                AddStatusToTheList(startStatus, firstLayerStatus);

                // 2. cikls - viens no ceļotājiem P2 iet atpakaļ uz P1, lai paņemtu trešo ceļotāju.

                for (int j = 0; j < firstLayerStatus.adventurersCrossed.Count; j++)
                {
                    Status secondLayerStatus = new Status(firstLayerStatus);

                    Adventurer adventurerToGoBack = secondLayerStatus.adventurersCrossed.ElementAt(j);

                    MoveAdventurerToP1(secondLayerStatus, adventurerToGoBack);

                    secondLayerStatus.SetTimeLeft(secondLayerStatus.GetTimeSpent() - GetTimeUponMoving(adventurerToGoBack));

                    AddStatusToTheList(firstLayerStatus, secondLayerStatus);

                    // Atlikušo ceļotāju pāreja no P1 uz P2.

                    Status thirdLayerStatus = new Status(secondLayerStatus);

                    firstAdventurerToMove = thirdLayerStatus.adventurersWaiting.ElementAt(0);
                    secondAdventurerToMove = thirdLayerStatus.adventurersWaiting.ElementAt(1);

                    MoveAdventurersToP2(thirdLayerStatus, firstAdventurerToMove, secondAdventurerToMove);

                    thirdLayerStatus.SetTimeLeft(thirdLayerStatus.GetTimeSpent() - GetTimeUponMoving(firstAdventurerToMove, secondAdventurerToMove));

                    AddStatusToTheList(secondLayerStatus, thirdLayerStatus);
                }
            }
        }

        /// <summary>
        /// Metode, lai pārvietotu 2 ceļotājus no P1 uz P2.
        /// </summary>
        /// <param name="status">Stāvoklis, kur tas notiks.</param>
        /// <param name="firstAdventurer">Pirmais ceļotājs, kuram jāpārvietojās.</param>
        /// <param name="secondAdventurer">Otrais ceļotājs, kuram jāpārvietojās.</param>
        public void MoveAdventurersToP2(Status status, Adventurer firstAdventurer, Adventurer secondAdventurer)
        {
            status.adventurersCrossed.Add(firstAdventurer);
            status.adventurersCrossed.Add(secondAdventurer);
            status.adventurersWaiting.Remove(firstAdventurer);
            status.adventurersWaiting.Remove(secondAdventurer);
        }

        /// <summary>
        /// Metode, lai pārvietotu ceļotāju no P2 uz P1.
        /// </summary>
        /// <param name="status">Stāvoklis, kur tas notiks.</param>
        /// <param name="adventurer">Ceļotajs, kuram jāpārvietojās.</param>
        public void MoveAdventurerToP1(Status status, Adventurer adventurer)
        {
            status.adventurersWaiting.Add(adventurer);
            status.adventurersCrossed.Remove(adventurer);
        }

        /// <summary>
        /// Metode, lai iegūtu laiku, cik aizņems pārvietošana 2 ceļotājiem..
        /// </summary>
        /// <param name="adv1">Pirmais ceļotājs, kuram jāpārvietojās.</param>
        /// <param name="adv2">Otrais ceļotājs, kuram jāpārvietojās.</param>
        /// <returns></returns>
        public int GetTimeUponMoving(Adventurer adv1, Adventurer adv2)
        {
            if (adv1.GetTimeToCross() > adv2.GetTimeToCross())
                return adv1.GetTimeToCross();
            else
                return adv2.GetTimeToCross();
        }

        /// <summary>
        /// Metode, lai iegūtu laiku, cik aizņems pārvietošana 1 ceļotājam.
        /// </summary>
        /// <param name="adv">Ceļotājs, kuram jāpārvietojās.</param>
        /// <returns></returns>
        public int GetTimeUponMoving(Adventurer adv)
        {
            return adv.GetTimeToCross();
        }

        /// <summary>
        /// Metode, lai pievienotu stāvokli sarakstam ar visiem izveidotiem stāvokļiem.
        /// </summary>
        /// <param name="existingStatus">Stāvoklis, kas jau eksistē, lai savienotu ar jaunu (ja jauns stāvoklis jau eksistē).</param>
        /// <param name="statusToAdd">Stāvoklis, kas neeksistē, lai pievienotu.</param>
        public void AddStatusToTheList(Status existingStatus, Status statusToAdd)
        {
            if(statusToAdd.GetTimeSpent() <= 0)
                return;

            foreach (Status status in listOfCreatedStatuses)
            {
                if (status.GetStatusInfo() == statusToAdd.GetStatusInfo())
                {
                    existingStatus.AddNextStatus(statusToAdd);
                    return;
                }
            }

            listOfCreatedStatuses.Add(statusToAdd);
        }

        /// <summary>
        /// Metode, kas attēlo visus eksistējošos stāvokļus sarakstā.
        /// </summary>
        public void PrintOutAllStatuses()
        {
            Console.WriteLine("Visi stāvokļi telpas stāvokļu grafā (pēc izveides momenta kārtas):");

            foreach(Status status in listOfCreatedStatuses)
            {
                Console.WriteLine(status.GetStatusInfo());
            }
        }

        /// <summary>
        /// "Hardcode" metode, kas izvada grafu uz ekrāna. Derīgs tikai vienam un vienīgam grafam.

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
                "\n\t\t\t1;;ABC\t\t3;;ABC");
            Console.WriteLine();
        }
    }
}
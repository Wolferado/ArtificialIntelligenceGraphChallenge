using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialIntelligenceGraphChallenge
{
    internal class Status
    {
        // Iepriekšējais stāvoklis grafam.
        private List<Status>? previousStatuses = new List<Status>();
        // Nākamais stāvoklis grafam.
        private List<Status>? nextStatuses = new List<Status>();

        // Lāpas degšanas laiks, kas tika izmantots.
        private int timeSpent = 0;
        // Simbolu virkne, kas skaidro, kāds ir stāvoklis.
        private string statusInfo = string.Empty;

        // Saraksts, kurš glab ceļotājus, kuri atrodas stāvokļa P1.
        public List<Adventurer> adventurersWaiting = new List<Adventurer>();
        // Sarakss, kurš glab ceļotājus, kuri atrodas stāvokļa P2.
        public List<Adventurer> adventurersCrossed = new List<Adventurer>();

        /// <summary>
        /// Konstruktors stāvoklim.
        /// </summary>
        public Status()
        {

        }

        /// <summary>
        /// Konstruktors stāvoklim, kas pieņem kā parametru citu stāvokli.
        /// </summary>
        /// <param name="status">Stāvoklis, kas tiek izmantots, lai izveidotu jaunu stāvokli.</param>
        public Status(Status status)
        {
            status.nextStatuses.Add(this);
            previousStatuses.Add(status);
            this.timeSpent = status.timeSpent;
            this.adventurersWaiting = new List<Adventurer>(status.adventurersWaiting);
            this.adventurersCrossed = new List<Adventurer>(status.adventurersCrossed);
        }

        /// <summary>
        /// Metode, kas uzstāda jaunu laiku.
        /// </summary>
        /// <param name="time">Laiks, kas tiek uzstādīts.</param>
        public void SetTimeLeft(int time)
        {
            timeSpent = time;
            UpdateStatusInfo();
        }

        /// <summary>
        /// Metode, lai iegūtu laiku.
        /// </summary>
        /// <returns>Laiks, kas ir saglabāts stāvoklī.</returns>
        public int GetTimeSpent()
        {
            return timeSpent;
        }

        /// <summary>
        /// Metode, lai iegūtu nākamo stāvokli (stāvokļus).
        /// </summary>
        /// <returns>Nākamais stāvoklis (stāvokli) sarakstā veidā.</returns>
        public List<Status> GetNextStatus()
        {
            return nextStatuses;
        }

        /// <summary>
        /// Metode, lai pievienotu jaunu nākamo stāvokli.
        /// </summary>
        /// <param name="status">Stāvoklis, kuru pievienotu.</param>
        public void AddNextStatus(Status status)
        {
            nextStatuses.Add(status);
        }

        /// <summary>
        /// Metode, lai atjaunotu stāvokļa informāciju.
        /// </summary>
        private void UpdateStatusInfo()
        {
            string adventuresWaitingIDs = String.Empty;
            string adventurersCrossedIDs = String.Empty;

            foreach(Adventurer adv in adventurersWaiting)
            {
                adventuresWaitingIDs += adv.GetIdentificationLetter();
            }

            foreach (Adventurer adv in adventurersCrossed)
            {
                adventurersCrossedIDs += adv.GetIdentificationLetter();
            }

            // Sakārtot identifikatorus augošā secībā, lai būtu iespējams tos salidzīnāt.
            adventuresWaitingIDs = String.Concat(adventuresWaitingIDs.OrderBy(ch => ch));
            adventurersCrossedIDs = String.Concat(adventurersCrossedIDs.OrderBy(ch => ch));

            this.statusInfo = String.Format("{0};{1};{2}", timeSpent, adventuresWaitingIDs, adventurersCrossedIDs);
        }

        /// <summary>
        /// Metode, lai iegūtu stāvokļa informāciju.
        /// </summary>
        /// <returns>Stāvokļa informācija kā simbolu virkne formātā: atlikušais laiks; objekti P1 vietā; objekti P2 vietā. Pīemērs: 12;AB;C .</returns>
        public string GetStatusInfo()
        {
            return statusInfo;
        }
    }
}

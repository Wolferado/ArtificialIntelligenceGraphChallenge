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
        private Status previousStatus;
        // Nākamais stāvoklis grafam.
        private Status nextStatus;

        // Lāpas degšanas laiks, kas tika izmantots.
        private int timeSpent = 0;
        // Simbolu virkne, kas skaidro, kāds ir simbolisks stāvoklis.
        private string statusInfo = string.Empty;

        // Saraksts, kurš glab ceļotājus, kuri atrodas stāvokļa P1.
        public List<Adventurer> adventurersWaiting = new List<Adventurer>();
        // Sarakss, kurš glab ceļotājus, kuri atrodas stāvokļa P2.
        public List<Adventurer> adventurersCrossed = new List<Adventurer>();

        public Status()
        {

        }

        public Status(Status previousStatus)
        {
            this.previousStatus = previousStatus;
            this.timeSpent = previousStatus.timeSpent;
            this.adventurersWaiting = previousStatus.adventurersWaiting;
            this.adventurersCrossed = previousStatus.adventurersCrossed;
        }

        public void SetTimeSpent(int time)
        {
            timeSpent = time;
            SetStatusInfo();
        }

        public int GetTimeSpent()
        {
            return timeSpent;
        }

        public void SetNextStatus(Status status)
        {
            this.nextStatus = status;
        }

        private void SetStatusInfo()
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

            this.statusInfo = String.Format("{0};{1};{2}", timeSpent, adventuresWaitingIDs, adventurersCrossedIDs);
        }

        public string GetStatusInfo()
        {
            return statusInfo;
        }
    }
}

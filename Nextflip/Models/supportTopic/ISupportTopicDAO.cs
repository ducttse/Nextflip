using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nextflip.Models.supportTopic
{
   public interface ISupportTopicDAO
    {
        public IList<SupportTopic> GetAllTopics();
    }
}

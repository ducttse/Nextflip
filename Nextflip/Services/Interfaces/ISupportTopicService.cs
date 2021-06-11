using Nextflip.Models.supportTopic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Interfaces
{
    public interface ISupportTopicService
    {
        public IList<SupportTopic> GetAllTopics();
    }
}

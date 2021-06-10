using Nextflip.Models.supportTopic;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nextflip.Services.Implementations
{
    public class SupportTopicService : ISupportTopicService
    {
        private readonly ISupportTopicDAO _supportTopicDAO;
        public SupportTopicService(ISupportTopicDAO supportTopicDAO)
        {
            _supportTopicDAO = supportTopicDAO;
        }
        public IList<SupportTopic> GetAllTopics() => _supportTopicDAO.GetAllTopics();
    }
}

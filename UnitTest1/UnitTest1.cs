using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.Service;
using System.Threading.Tasks;

namespace UnitTest1
{
    [TestClass]
    public class UnitTest1
    {
        private AdminService adminService = new AdminService();
        [TestMethod]
        public async Task TestMethod1()
        {
            await adminService.DelAll();
        }
    }
}

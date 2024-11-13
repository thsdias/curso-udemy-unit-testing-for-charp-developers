using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinjaUnitTests.Mocking
{
    [TestFixture]
    public class HousekeeperServiceTests
    {
        private Mock<IStatementGenerator> _statementGenerator;
        private Mock<IEmailSender> _emailSender;
        private Mock<IXtraMessageBox> _messageBox;
        private HousekeeperService _service;
        private readonly DateTime _statementDate = (new DateTime(2017, 1, 1));
        private Housekeeper _housekeeper;
        private string _statementFilename;

        [SetUp]
        public void Setup()
        {
            _housekeeper = new Housekeeper { Email = "a", FullName = "b", Oid = 1, StatementEmailBody = "c" };
            
            var unitOfWork = new Mock<IUnitOfWork>();

            unitOfWork.Setup(uow => 
                    uow.Query<Housekeeper>())
                        .Returns(new List<Housekeeper>
                        {
                            _housekeeper
                        }.AsQueryable());

            _statementFilename = "fileName";
            _statementGenerator = new Mock<IStatementGenerator>();
            _statementGenerator
                .Setup(sg => sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate))
                .Returns(() => _statementFilename);
            
            _emailSender = new Mock<IEmailSender>();
            _messageBox = new Mock<IXtraMessageBox>();
            
            _service = new HousekeeperService(
                unitOfWork.Object, 
                _statementGenerator.Object, 
                _emailSender.Object, 
                _messageBox.Object);
        }
        
        [Test]
        public void SendStatementEmails_WhenCalled_GenerateStatements()
        {
            _service.SendStatementEmails(_statementDate);
            
            _statementGenerator.Verify(sg => 
                sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate));
        }
        
        [Test]
        public void SendStatementEmails_HouseKeepersEmailIsNUll_ShouldNotGenerateStatements()
        {
            _housekeeper.Email = null;
            
            _service.SendStatementEmails(_statementDate);
            
            _statementGenerator.Verify(sg => 
                sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate),
                Times.Never);
        }
        
        [Test]
        public void SendStatementEmails_HouseKeepersEmailIsWhitespace_ShouldNotGenerateStatements()
        {
            _housekeeper.Email = " ";
            
            _service.SendStatementEmails(_statementDate);
            
            _statementGenerator.Verify(sg => 
                    sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate),
                    Times.Never);
        }
        
        [Test]
        public void SendStatementEmails_HouseKeepersEmailIsEmpty_ShouldNotGenerateStatements()
        {
            _housekeeper.Email = "";
            
            _service.SendStatementEmails(_statementDate);
            
            _statementGenerator.Verify(sg => 
                    sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate),
                    Times.Never);
        }
        
        [Test]
        public void SendStatementEmails_WhenCalled_EmailTheStatement()
        {
            _statementGenerator
                .Setup(sg => sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate))
                .Returns(_statementFilename);

            _service.SendStatementEmails(_statementDate);
            
            _emailSender.Verify(es => es.EmailFile(
                _housekeeper.Email,
                _housekeeper.StatementEmailBody,
                _statementFilename,
                It.IsAny<string>())
            );
        }
        
        [Test]
        public void SendStatementEmails_StatementFileNameIsNUll_ShouldNotEmailTheStatement()
        {
            _statementFilename = null;
            
            // Look Setup:
            // _statementGenerator
            //     .Setup(sg => sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate))
            //     .Returns(() => null);

            _service.SendStatementEmails(_statementDate);
            
            VerifyEmailNotSend();
        }
        
        [Test]
        public void SendStatementEmails_StatementFileNameIsEmptyString_ShouldNotEmailTheStatement()
        {
            _statementFilename = "";
            
            // Look Setup:
            // _statementGenerator
            //     .Setup(sg => sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate))
            //     .Returns("");

            _service.SendStatementEmails(_statementDate);

            VerifyEmailNotSend();
        }
        
        [Test]
        public void SendStatementEmails_StatementFileNameIsWhitespace_ShouldNotEmailTheStatement()
        {
            _statementFilename = " ";
            
            // Look Setup:
            // _statementGenerator
            //     .Setup(sg => sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate))
            //     .Returns(" ");

            _service.SendStatementEmails(_statementDate);
            
            VerifyEmailNotSend();
        }

        [Test]
        public void SendStatementEmails_EmailSendingFails_DisplayAMessageBox()
        {
            _emailSender.Setup(es => 
                es.EmailFile(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()
                )).Throws<Exception>();
            
            _service.SendStatementEmails(_statementDate);
            
            _messageBox.Verify(mb => mb.Show(It.IsAny<string>(), It.IsAny<string>(), MessageBoxButtons.OK));
        }
        
        private void VerifyEmailNotSend()
        {
            _emailSender.Verify(es => es.EmailFile(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never
            );
        }
    }
}
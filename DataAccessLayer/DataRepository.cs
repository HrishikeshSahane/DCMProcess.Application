using DCMProcess.DataAccessLayer;


namespace DCMProcess.DataAccessLayer
{
    public class DataRepository
    {
        private readonly DbprojectContext _context;

        public DataRepository(DbprojectContext context)
        {
            _context = context;
        }

        public string SavePatient(Patient patient)
        {

            try
            {
                var x = (from p in _context.Patients where p.PatientId == patient.PatientId select p).FirstOrDefault();
                if (x == null)
                {
                     patient.Gender = patient.Gender.ToLower();
                    _context.Patients.Add(patient);
                    _context.SaveChanges();
                    return "Success";
                }
                return "Null";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public Patient GetPatient(string patientId)
        {
            Patient patient = new Patient();
            try
            {
                
                patient=(from p in _context.Patients where p.PatientId==patientId select p).FirstOrDefault();
                if (patient != null)
                {
                    return patient;
                }
                return patient;
            }
            catch
            {
                return patient;
            }
        }

        public List<Patient> GetAllPatients()
        {
            List<Patient> patients = new List<Patient>();
            try
            {
                patients= (from p in _context.Patients select p).ToList();
                if (patients != null)
                {
                    return patients;
                }
                return patients;
            }
            catch( Exception ex )
            {
                return patients;
            }
        }
        public bool SaveScanDetails(ScanDetail scanDetails, string patientMRN)
        {
            try
            {
                var x = (from p in _context.Patients where p.PatientId == patientMRN select p).FirstOrDefault();
                if (x != null)
                {
                    scanDetails.PatientId = x.Id;
                    var y = (from s in _context.ScanDetails where s.SeriesId == scanDetails.SeriesId select s).FirstOrDefault();
                    if (y == null)
                    {
                        _context.ScanDetails.Add(scanDetails);
                        _context.SaveChanges();
                        return true;
                    }

                }
                return false;
            }
            catch
            {
                return true;
            }
        }

        public List<ScanDetail> GetScanDetails(string patientId)
        {
            List<ScanDetail> scanDetailsList = new List<ScanDetail>();
            try
            {
                var patient = (from p in _context.Patients where p.PatientId == patientId select p).FirstOrDefault();
                if (patient != null)
                {

                    scanDetailsList = (from s in _context.ScanDetails where s.PatientId == patient.Id select s).ToList();
                    return scanDetailsList;
                }
                else
                {
                    return scanDetailsList;
                }
            }
            catch
            {
                return scanDetailsList;
            }
        }

        public bool SaveInstitutionDetails(InstitutionDetail institutionDetails, string imageSeriesId)
        {
            try
            {
                var x = (from s in _context.ScanDetails where s.SeriesId == imageSeriesId select s).FirstOrDefault();
                if (x != null)
                {
                    institutionDetails.SeriesId = x.Id.ToString();
                    var y = (from i in _context.InstitutionDetails where i.SeriesId == x.Id.ToString() select i).FirstOrDefault();
                    if (y == null)
                    {
                        _context.InstitutionDetails.Add(institutionDetails);
                        _context.SaveChanges();
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public InstitutionDetail GetInstitutionDetail(string patientId)
        {
            InstitutionDetail institutionDetail = new InstitutionDetail();
            try
            {
                var patient = (from p in _context.Patients where p.PatientId == patientId select p).FirstOrDefault();
                if (patient != null)
                {
                    ScanDetail scanDetails = (from s in _context.ScanDetails where s.PatientId == patient.Id select s).FirstOrDefault();
                    if (scanDetails != null)
                    {
                        institutionDetail=(from i in _context.InstitutionDetails where i.SeriesId==scanDetails.Id.ToString() select i).FirstOrDefault();

                    }

                }
                return institutionDetail;
            }
            catch(Exception ex)
            {
                return institutionDetail;
            }
        }

        public bool MachineScannerDetails(MachineDetail machineDetail, string imageSeriesId)
        {
            try
            {
                var x = (from s in _context.ScanDetails where s.SeriesId == imageSeriesId select s).FirstOrDefault();
                if (x != null)
                {
                    machineDetail.SeriesId = x.Id;
                    var y = (from i in _context.MachineDetails where i.SeriesId == x.Id select i).FirstOrDefault();
                    if (y == null)
                    {
                        _context.MachineDetails.Add(machineDetail);
                        _context.SaveChanges();
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }


        public MachineDetail GetMachineScannerDetail(string patientId)
        {
            MachineDetail machineDetail = new MachineDetail();
            try
            {
                var patient = (from p in _context.Patients where p.PatientId == patientId select p).FirstOrDefault();
                if (patient != null)
                {
                    ScanDetail scanDetails = (from s in _context.ScanDetails where s.PatientId == patient.Id select s).FirstOrDefault();
                    if (scanDetails != null)
                    {
                        machineDetail = (from m in _context.MachineDetails where (m.SeriesId) == (scanDetails.Id) select m).FirstOrDefault();

                    }

                }
                return machineDetail;
            }
            catch (Exception ex)
            {
                return machineDetail;
            }
        }

        public bool SaveQueueDetails(QueueDetail queueDetails)
        {
            try
            {
                _context.QueueDetails.Add(queueDetails);
                _context.SaveChanges();
                return true;
            }
            catch(Exception ex) 
            {
                return false;
            }
        }

        public byte? GetRoleIdByUserId(string userId)
        {
            try
            {
                var roleId = _context.Users.Where(x => x.EmailId == userId).Select(x => x.RoleId).FirstOrDefault();
                //var roleName = context.Roles.Where(x => x.RoleId == roleId).Select(x => x.RoleName).FirstOrDefault();
                return roleId;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public string GetRoleName(int roleId)
        {
            try
            {
                var roleName = _context.Roles.Where(x => x.RoleId == roleId).Select(x => x.RoleName).FirstOrDefault();
                return roleName;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public byte? ValidateCredentials(string userId, string password)
        {

            try
            {
                User users = _context.Users.Find(userId);
                byte? roleId = null;
                if (users.UserPassword == password)
                {
                    roleId = users.RoleId;
                }
                else
                {
                    roleId = 0;
                }

                return roleId;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public string ValidateLoginUsingLinq(string emailId, string password)
        {

            string roleName = "";

            try
            {
                 
                var objUser = (from usr in _context.Users
                               where usr.EmailId == emailId && usr.UserPassword==password
                               select usr).ToList();
                var user=objUser.FirstOrDefault(x=>string.Equals(x.UserPassword,password, StringComparison.Ordinal));

                if (user != null)
                {

                    roleName = (from role in _context.Roles
                                where role.RoleId == user.RoleId
                                select role.RoleName).FirstOrDefault();
                }
                else
                {
                    roleName = "Invalid credentials";
                }
            }
            catch (Exception ex)
            {
                roleName = "Some error Occured: "+ ex.Message;
            }
            return roleName;
        }

        public User GetUser(string emailId)
        {
            User user = new User();
            user = (from c in _context.Users where c.EmailId == emailId select c).First();
            return user;
        }

        public string RegisterNewUser(User registerUser)
        {
            try
            {
                var user=(from x in _context.Users where x.EmailId==registerUser.EmailId || x.LastName==registerUser.LastName select x).FirstOrDefault<User>();
                if (user == null)
                {
                    _context.Users.Add(registerUser);
                    _context.SaveChanges();
                    return "Success";
                }
                else
                {
                    return "User with same emailId or LastName already exists";
                }
            }
            catch(Exception ex)
            {
                return $"Some error occured: {ex.Message}";
            }
        }
    }
}

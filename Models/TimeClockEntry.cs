using Csla;
using System;
using TimeClockApi.Dal;
using System.Threading.Tasks;

namespace TimeClockApi.Models
{
    [Serializable]
    public class TimeClockEntry : BusinessBase<TimeClockEntry>
    {
        public static readonly PropertyInfo<int> IdProperty = RegisterProperty<int>(nameof(Id));
        public int Id
        {
            get => GetProperty(IdProperty);
            private set => SetProperty(IdProperty, value);
        }

        public static readonly PropertyInfo<int> EmployeeIdProperty = RegisterProperty<int>(nameof(EmployeeId));
        public int EmployeeId
        {
            get => GetProperty(EmployeeIdProperty);
            set => SetProperty(EmployeeIdProperty, value);
        }

        public static readonly PropertyInfo<DateTime> ClockInTimeProperty = RegisterProperty<DateTime>(nameof(ClockInTime));
        public DateTime ClockInTime
        {
            get => GetProperty(ClockInTimeProperty);
            set => SetProperty(ClockInTimeProperty, value);
        }

        public static readonly PropertyInfo<DateTime?> ClockOutTimeProperty = RegisterProperty<DateTime?>(nameof(ClockOutTime));
        public DateTime? ClockOutTime
        {
            get => GetProperty(ClockOutTimeProperty);
            set => SetProperty(ClockOutTimeProperty, value);
        }

        public static readonly PropertyInfo<string> LocationProperty = RegisterProperty<string>(nameof(Location));
        public string Location
        {
            get => GetProperty(LocationProperty);
            set => SetProperty(LocationProperty, value);
        }

        [Create]
        private void Create()
        {
            // Creation rules here if needed
        }

        [CreateChild]
        private void CreateChild()
        {
            // Child creation rules here if needed
        }

        [Fetch]
        private void Fetch(int id, [Inject] ITimeClockDal dal)
        {
            var data = dal.GetEntry(id);
            using (BypassPropertyChecks)
            {
                Id = data.Id;
                EmployeeId = data.EmployeeId;
                ClockInTime = data.ClockInTime;
                ClockOutTime = data.ClockOutTime;
                Location = data.Location;
            }
        }

        [Insert]
        private void Insert([Inject] ITimeClockDal dal)
        {
            using (BypassPropertyChecks)
            {
                var data = dal.Insert(this.EmployeeId, this.ClockInTime, this.Location);
                Id = data.Id;
            }
        }

        [Update]
        private void Update([Inject] ITimeClockDal dal)
        {
            using (BypassPropertyChecks)
            {
                if (this.ClockOutTime.HasValue)
                {
                    dal.Update(this.Id, this.ClockOutTime.Value);
                }
                else
                {
                    throw new InvalidOperationException("ClockOutTime must have a value");
                }
            }
        }

        public static TimeClockEntry NewTimeClockEntry()
        {
            return BusinessBase<TimeClockEntry>.Factory.CreateChild();
        }

        internal void LoadFromDto(TimeClockEntryDto dto)
        {
            using (BypassPropertyChecks)
            {
                Id = dto.Id;
                EmployeeId = dto.EmployeeId;
                ClockInTime = dto.ClockInTime;
                ClockOutTime = dto.ClockOutTime;
                Location = dto.Location;
            }
        }
    }
} 
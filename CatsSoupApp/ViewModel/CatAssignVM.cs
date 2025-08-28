using CatsSoupApp.Model;
using CatsSoupApp.ViewModel.BaseClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CatsSoupApp.ViewModel
{
    class CatAssignVM : ViewModelBase
    {
        private readonly MainVM _mainVM;
        public ObservableCollection<Cat> Cats { get; }
        public FacilityCollection FacilityCollection { get; }
        public StationAssigner StationAssigner { get; }

        #region Binding
        private Facility _lastFacility;
        public Facility LastFacility
        {
            get => _lastFacility;
            set { _lastFacility = value; OnPropertyChanged(); }
        }

        private string _assignResult;
        public string AssignResult
        {
            get => _assignResult;
            set
            {
                if (_assignResult != value)
                {
                    _assignResult = value;
                    OnPropertyChanged(nameof(AssignResult));
                }
            }
        }
        #endregion

        public ICommand SwitchToListView => _mainVM.SwitchToListView;

        private ICommand _runAssign;
        public ICommand RunAssign => _runAssign ??= new RelayCommand(_ => RunAssignment(Cats, LastFacility), p => LastFacility != null);

        public CatAssignVM(MainVM mainVM, ObservableCollection<Cat> cats, FacilityCollection facilityCollection)
        {
            _mainVM = mainVM;
            Cats = cats;
            FacilityCollection = facilityCollection;

            StationAssigner = new StationAssigner(FacilityCollection);
        }

        public void RunAssignment(ObservableCollection<Cat> Cats, Facility LastFacility)
        {
            string lastUnlockedStation = LastFacility._name;
            AssignResult = StationAssigner.AssignCats(Cats, lastUnlockedStation);
        }
    }
}

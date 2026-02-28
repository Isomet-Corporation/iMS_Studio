using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using iMS_Studio.ViewModel.Behaviour;
using System.Windows.Input;

namespace iMS_Studio.ViewModel
{
    public enum CrystalTypesVM
    {
        [Description("Lead Molybdate (PbMoO4)")]
        PbMoO4,
        [Description("Tellurium Dioxide (TeO2)")]
        TeO2,
        [Description("Tellurium Dioxide Shear Mode (TeO2-S)")]
        TeO2S,
        [Description("Alpha-Quartz")]
        aQuartz,
        [Description("Fused Silica")]
        fSilica,
        [Description("Fused Silica (Shear Mode)")]
        fSilicaS,
        [Description("Germanium")]
        Ge
    };


    class AODeviceChooserVM : BaseViewModel, IDialogResultVMHelper
    {
        private readonly Lazy<RelayCommand> _okCommand;
        private readonly Lazy<RelayCommand> _cancelCommand;

        // The model
        private iMS.AODevice aod;
        private iMS.Micrometre targetWavelength;

        public iMS.AODevice AODModel { get { return aod; } }
        public iMS.Micrometre TargetWavelength { get { return targetWavelength; } }

        public AODeviceChooserVM()
        {
            this._okCommand = new Lazy<RelayCommand>(() =>
                new RelayCommand(dlg =>
                    InvokeRequestCloseDialog(
                        new RequestCloseDialogEventArgs(true))) );
            this._cancelCommand = new Lazy<RelayCommand>(() =>
                new RelayCommand(dlg =>
                    InvokeRequestCloseDialog(
                        new RequestCloseDialogEventArgs(false))));

            // Start off on first device in list
            SelectedAOD = 0;
            CustomAOD = false;
        }

        public ICommand OkCommand
        {
            get { return this._okCommand.Value; }
        }

        public ICommand CancelCommand
        {
            get { return this._cancelCommand.Value; }
        }

        public event EventHandler<RequestCloseDialogEventArgs> RequestCloseDialog;
        private void InvokeRequestCloseDialog(RequestCloseDialogEventArgs e)
        {
            RequestCloseDialog?.Invoke(this, e);
        }

        private double DegreesToMilliRads(double value)
        {
            return ((value * Math.PI) / 0.18);
        }

        private void updateConstants()
        {
            iMS.Crystal xtal = new iMS.Crystal();
            switch (_crystalType)
            {
                case CrystalTypesVM.aQuartz: xtal = new iMS.Crystal(iMS.Crystal.Material.aQuartz); break;
                case CrystalTypesVM.fSilica: xtal = new iMS.Crystal(iMS.Crystal.Material.fSilica); break;
                case CrystalTypesVM.fSilicaS: xtal = new iMS.Crystal(iMS.Crystal.Material.fSilicaS); break;
                case CrystalTypesVM.Ge: xtal = new iMS.Crystal(iMS.Crystal.Material.Ge); break;
                case CrystalTypesVM.PbMoO4: xtal = new iMS.Crystal(iMS.Crystal.Material.PbMoO4); break;
                case CrystalTypesVM.TeO2: xtal = new iMS.Crystal(iMS.Crystal.Material.TeO2); break;
                case CrystalTypesVM.TeO2S: xtal = new iMS.Crystal(iMS.Crystal.Material.TeO2S); break;
            }
            AcousticVelocity = xtal.AcousticVelocity;
            ExternalBragg = DegreesToMilliRads(aod.ExternalBragg(new iMS.Micrometre(OpWavelength)).Value);
//            ExternalBragg = xtal.BraggAngle(new iMS.Micrometre(OpWavelength), new iMS.MHz(CentreFrequency)).Value;
        }

        private void updateModel()
        {
            if (CustomAOD)
            {
                iMS.Crystal xtal = new iMS.Crystal();
                switch (_crystalType)
                {
                    case CrystalTypesVM.aQuartz: xtal = new iMS.Crystal(iMS.Crystal.Material.aQuartz); break;
                    case CrystalTypesVM.fSilica: xtal = new iMS.Crystal(iMS.Crystal.Material.fSilica); break;
                    case CrystalTypesVM.fSilicaS: xtal = new iMS.Crystal(iMS.Crystal.Material.fSilicaS); break;
                    case CrystalTypesVM.Ge: xtal = new iMS.Crystal(iMS.Crystal.Material.Ge); break;
                    case CrystalTypesVM.PbMoO4: xtal = new iMS.Crystal(iMS.Crystal.Material.PbMoO4); break;
                    case CrystalTypesVM.TeO2: xtal = new iMS.Crystal(iMS.Crystal.Material.TeO2); break;
                    case CrystalTypesVM.TeO2S: xtal = new iMS.Crystal(iMS.Crystal.Material.TeO2S); break;
                }
                aod = new iMS.AODevice(xtal, GeomConstant, new iMS.MHz(CentreFrequency), new iMS.MHz(SweepBandwidth));
            }
            else
            {
                aod = new iMS.AODevice(AODevList[SelectedAOD]);
            }
            targetWavelength = new iMS.Micrometre(OpWavelength);
        }

        public iMS.StringList AODevList {
            get { return iMS.AODeviceList.GetList(); }
        }

        private int _selectedAOD = -1;
        public int SelectedAOD
        {
            get
            { return _selectedAOD; }
            set
            {
                if ((value != _selectedAOD) &&
                    (value >= 0) &&
                    (value < iMS.AODeviceList.GetList().size()))
                {
                    _selectedAOD = value;

                    updateModel();
                    iMS.Crystal xtal = aod.Material;
                    switch(xtal.Type)
                    {
                        case iMS.Crystal.Material.aQuartz: CrystalType = CrystalTypesVM.aQuartz; break;
                        case iMS.Crystal.Material.fSilica: CrystalType = CrystalTypesVM.fSilica; break;
                        case iMS.Crystal.Material.fSilicaS: CrystalType = CrystalTypesVM.fSilicaS; break;
                        case iMS.Crystal.Material.Ge: CrystalType = CrystalTypesVM.Ge; break;
                        case iMS.Crystal.Material.PbMoO4: CrystalType = CrystalTypesVM.PbMoO4; break;
                        case iMS.Crystal.Material.TeO2: CrystalType = CrystalTypesVM.TeO2; break;
                        case iMS.Crystal.Material.TeO2S: CrystalType = CrystalTypesVM.TeO2S; break;
                    }
                    CentreFrequency = aod.CentreFrequency.Value;
                    SweepBandwidth = aod.SweepBW.Value;
                    OpWavelength = aod.OperatingWavelength.Value;
                    GeomConstant = aod.GeomConstant;

                    OnPropertyChanged("SelectedAOD");
                }
            }
        }

        private CrystalTypesVM _crystalType;
        public CrystalTypesVM CrystalType
        {
            get { return _crystalType; }
            set
            {
                if (value != _crystalType)
                {
                    _crystalType = value;
                    updateModel();
                    updateConstants();
                    OnPropertyChanged("CrystalType");
                }
            }
        }

        private double _centreFrequency;
        public double CentreFrequency
        {
            get { return _centreFrequency; }
            set
            {
                if (value != _centreFrequency)
                {
                    _centreFrequency = value;
                    updateModel();
                    //updateConstants();
                    OnPropertyChanged("CentreFrequency");
                }
            }
        }

        private double _sweepBandwidth;
        public double SweepBandwidth
        {
            get { return _sweepBandwidth; }
            set
            {
                if (value != _sweepBandwidth)
                {
                    _sweepBandwidth = value;
                    updateModel();
                    OnPropertyChanged("SweepBandwidth");
                }
            }
        }

        private double _opWavelength;
        public double OpWavelength
        {
            get { return _opWavelength; }
            set
            {
                if (value != _opWavelength)
                {
                    _opWavelength = value;
                    updateModel();
                    updateConstants();
                    OnPropertyChanged("OpWavelength");
                }
            }
        }

        private double _geomConstant;
        public double GeomConstant
        {
            get { return _geomConstant; }
            set
            {
                if (value != _geomConstant)
                {
                    _geomConstant = value;
                    updateModel();
                    OnPropertyChanged("GeomConstant");
                }
            }
        }

        private double _acousticVelocity;
        public double AcousticVelocity
        {
            get { return _acousticVelocity; }
            set
            {
                if (value != _acousticVelocity)
                {
                    _acousticVelocity = value;
                    OnPropertyChanged("AcousticVelocity");
                }
            }
        }

        private double _externalBragg;
        public double ExternalBragg
        {
            get { return _externalBragg; }
            set
            {
                if (value != _externalBragg)
                {
                    _externalBragg = value;
                    OnPropertyChanged("ExternalBragg");
                }
            }
        }

        private bool _customAOD;
        public bool CustomAOD
        {
            get { return _customAOD; }
            set
            {
                if (value != _customAOD)
                {
                    _customAOD = value;
                    OnPropertyChanged("CustomAOD");
                }
            }
        }
    }
}

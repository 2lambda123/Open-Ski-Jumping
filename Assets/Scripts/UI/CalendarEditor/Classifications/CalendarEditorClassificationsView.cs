using System;
using System.Collections.Generic;
using System.Linq;
using OpenSkiJumping.Competition.Persistent;
using OpenSkiJumping.Data;
using OpenSkiJumping.ScriptableObjects;
using OpenSkiJumping.UI.ListView;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace OpenSkiJumping.UI.CalendarEditor.Classifications
{
    public class CalendarEditorClassificationsView : MonoBehaviour, ICalendarEditorClassificationsView
    {
        private bool initialized;
        private CalendarEditorClassificationsPresenter presenter;
        [SerializeField] private CalendarFactory calendarFactory;
        [SerializeField] private PointsTablesRuntime pointsTablesData;
        [SerializeField] private IconsData iconsData;

        [Header("UI Fields")] [SerializeField] private ClassificationsListView listView;
        [SerializeField] private GameObject classificationInfoObj;
        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private SegmentedControl eventTypeSelect;
        [SerializeField] private SegmentedControl classificationTypeSelect;
        [SerializeField] private TMP_Dropdown indPointsTableDropdown;
        [SerializeField] private TMP_Dropdown teamPointsTableDropdown;
        [SerializeField] private TMP_InputField limitInput;
        [SerializeField] private SegmentedControl limitTypeSelect;
        [SerializeField] private TMP_InputField medalPlacesInput;
        [SerializeField] private SimpleColorPicker bibColor;

        [SerializeField] private GameObject indTableObj;
        [SerializeField] private GameObject teamTableObj;
        [SerializeField] private GameObject limitObj;
        [SerializeField] private GameObject medalsObj;

        [SerializeField] private Button addButton;
        [SerializeField] private Button removeButton;

        private List<PointsTable> pointsTablesIndividual;
        private List<PointsTable> pointsTablesTeam;
        private List<ClassificationInfo> classifications;

        public ClassificationInfo SelectedClassification
        {
            get => listView.SelectedIndex < 0 ? null : classifications[listView.SelectedIndex];
            set => SelectClassification(value);
        }

        public IEnumerable<ClassificationInfo> Classifications
        {
            set
            {
                classifications = value.ToList();
                listView.Items = classifications;
                listView.ClampSelectedIndex();
                listView.Refresh();
            }
        }

        public IEnumerable<PointsTable> PointsTablesIndividual
        {
            set
            {
                pointsTablesIndividual = value.ToList();

                indPointsTableDropdown.ClearOptions();
                indPointsTableDropdown.AddOptions(pointsTablesIndividual.Select(item => item.name).ToList());
            }
        }

        public IEnumerable<PointsTable> PointsTablesTeam
        {
            set
            {
                pointsTablesTeam = value.ToList();
                teamPointsTableDropdown.ClearOptions();
                teamPointsTableDropdown.AddOptions(pointsTablesTeam.Select(item => item.name).ToList());
            }
        }

        public PointsTable SelectedPointsTableIndividual
        {
            get => pointsTablesIndividual[indPointsTableDropdown.value];
            set => SelectPointsTable(value, indPointsTableDropdown, pointsTablesIndividual);
        }

        public PointsTable SelectedPointsTableTeam
        {
            get => pointsTablesTeam[teamPointsTableDropdown.value];
            set => SelectPointsTable(value, teamPointsTableDropdown, pointsTablesTeam);
        }

        public string Name
        {
            get => nameInput.text;
            set => nameInput.SetTextWithoutNotify(value);
        }

        public int EventType
        {
            get => eventTypeSelect.selectedSegmentIndex;
            set => eventTypeSelect.SetSelectedSegmentWithoutNotify(value);
        }

        public int ClassificationType
        {
            get => classificationTypeSelect.selectedSegmentIndex;
            set => classificationTypeSelect.SetSelectedSegmentWithoutNotify(value);
        }

        public int TeamClassificationLimitType
        {
            get => limitTypeSelect.selectedSegmentIndex;
            set
            {
                if (limitTypeSelect.gameObject.activeSelf) limitTypeSelect.SetSelectedSegmentWithoutNotify(value);
            }
        }

        public int TeamClassificationLimit
        {
            get => int.Parse(limitInput.text);
            set => limitInput.SetTextWithoutNotify(value.ToString());
        }

        public int MedalPlaces
        {
            get => int.Parse(medalPlacesInput.text);
            set => medalPlacesInput.SetTextWithoutNotify(value.ToString());
        }

        public string BibColor
        {
            get => bibColor.ToHex;
            set => bibColor.SetValueWithoutNotify(value);
        }

        public event Action OnSelectionChanged;
        public event Action OnCurrentClassificationChanged;
        public event Action OnAdd;
        public event Action OnRemove;
        public event Action OnDataReload;

        public bool ClassificationInfoEnabled
        {
            set
            {
                if (value) ShowClassificationInfo();
                else HideClassificationInfo();
            }
        }

        private void SelectPointsTable(PointsTable value, TMP_Dropdown dropdown, List<PointsTable> pointsTablesList)
        {
            var index = value == null ? dropdown.value : pointsTablesList.FindIndex(item => item.name == value.name);
            index = Mathf.Clamp(index, 0, pointsTablesList.Count - 1);
            dropdown.SetValueWithoutNotify(index);
        }

        public event Action OnDataSave;

        private void Start()
        {
            ListViewSetup();
            RegisterCallbacks();
            presenter = new CalendarEditorClassificationsPresenter(this, calendarFactory, pointsTablesData);
            initialized = true;
        }

        private void OnDisable()
        {
            OnDataSave?.Invoke();
        }

        private void ListViewSetup()
        {
            listView.OnSelectionChanged += x => OnSelectionChanged?.Invoke();
            listView.SelectionType = SelectionType.Single;
            listView.Initialize(BindListViewItem);
        }

        private void RegisterCallbacks()
        {
            addButton.onClick.AddListener(() => OnAdd?.Invoke());
            removeButton.onClick.AddListener(() => OnRemove?.Invoke());
            nameInput.onEndEdit.AddListener(arg => OnCurrentClassificationChanged?.Invoke());

            RegisterSegmentedControlCallbacks(eventTypeSelect);
            RegisterSegmentedControlCallbacks(classificationTypeSelect);
            RegisterSegmentedControlCallbacks(limitTypeSelect);

            limitInput.onEndEdit.AddListener(arg => OnCurrentClassificationChanged?.Invoke());
            medalPlacesInput.onEndEdit.AddListener(arg => OnCurrentClassificationChanged?.Invoke());
            indPointsTableDropdown.onValueChanged.AddListener(arg => OnCurrentClassificationChanged?.Invoke());
            teamPointsTableDropdown.onValueChanged.AddListener(arg => OnCurrentClassificationChanged?.Invoke());
            bibColor.OnColorChange += () => OnCurrentClassificationChanged?.Invoke();
        }

        private void RegisterSegmentedControlCallbacks(SegmentedControl item)
        {
            item.onValueChanged.AddListener(arg => OnCurrentClassificationChanged?.Invoke());
            item.onValueChanged.AddListener(arg => ShowClassificationInfo());
        }

        private void SelectClassification(ClassificationInfo classification)
        {
            listView.SelectedIndex =
                classification == null ? listView.SelectedIndex : classifications.IndexOf(classification);

            listView.ClampSelectedIndex();
            listView.ScrollToIndex(listView.SelectedIndex);
            listView.RefreshShownValue();
        }

        private void HideClassificationInfo() => classificationInfoObj.SetActive(false);

        private void ShowClassificationInfo()
        {
            classificationInfoObj.SetActive(true);
            if (SelectedClassification.classificationType == Competition.ClassificationType.Place)
            {
                indTableObj.SetActive(true);
                teamTableObj.SetActive(SelectedClassification.eventType == Competition.EventType.Team);
            }
            else
            {
                indTableObj.SetActive(false);
                teamTableObj.SetActive(false);
            }

            limitInput.gameObject.SetActive(SelectedClassification.teamClassificationLimitType ==
                                            Competition.Persistent.TeamClassificationLimitType.Best);
            limitObj.SetActive(SelectedClassification.eventType == Competition.EventType.Team);
            medalsObj.SetActive(SelectedClassification.classificationType == Competition.ClassificationType.Medal);
        }

        private void BindListViewItem(int index, ClassificationsListItem item)
        {
            var classificationInfo = classifications[index];
            item.nameText.text = classificationInfo.name;
            item.bibImage.color = SimpleColorPicker.Hex2Color(classificationInfo.leaderBibColor);
            item.classificationTypeImage.sprite = iconsData.GetClassificationTypeIcon(classificationInfo.classificationType);
            item.eventTypeImage.sprite = iconsData.GetEventTypeIcon(classificationInfo.eventType);
        }

        public void OnEnable()
        {
            if (!initialized) return;
            OnDataReload?.Invoke();
            listView.Reset();
        }
    }
}
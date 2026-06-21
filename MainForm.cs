using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GymMemberSystem
{
    public class MainForm : Form
    {
        private readonly Color PageBg = Color.FromArgb(12, 12, 12);
        private readonly Color NavBg = Color.FromArgb(5, 5, 5);
        private readonly Color NavActive = Color.FromArgb(250, 204, 21);
        private readonly Color NavHover = Color.FromArgb(31, 31, 31);
        private readonly Color CardBg = Color.FromArgb(24, 24, 27);
        private readonly Color InputBg = Color.FromArgb(39, 39, 42);
        private readonly Color TextMain = Color.FromArgb(250, 250, 250);
        private readonly Color TextSub = Color.FromArgb(212, 212, 216);
        private readonly Color Border = Color.FromArgb(63, 63, 70);
        private readonly Color Blue = Color.FromArgb(250, 204, 21);
        private readonly Color Teal = Color.FromArgb(234, 179, 8);
        private readonly Color Green = Color.FromArgb(202, 138, 4);
        private readonly Color Orange = Color.FromArgb(245, 158, 11);
        private readonly Color Red = Color.FromArgb(239, 68, 68);
        private readonly Color Purple = Color.FromArgb(161, 98, 7);

        private List<Member> members;
        private List<BodyRecord> bodyRecords;
        private List<PaymentRecord> payments;

        private Panel navPanel;
        private Panel headerPanel;
        private Panel contentPanel;
        private Label lblTitle;
        private Label lblSubtitle;
        private ComboBox cboHeaderMember;
        private readonly List<Button> navButtons = new List<Button>();
        private string currentPage = "Home";
        private bool isBinding;
        private int editingMemberId = -1;
        private string currentPhotoPath = string.Empty;
        private PictureBox picMemberPhoto;

        private TextBox txtMemberSearch;
        private TextBox txtMemberName;
        private TextBox txtMemberPhone;
        private TextBox txtMemberTrainer;
        private ComboBox cboMemberGender;
        private ComboBox cboMemberGoal;
        private DateTimePicker dtMemberBirthday;
        private DateTimePicker dtMemberJoin;
        private DateTimePicker dtMemberExpire;
        private NumericUpDown nudMemberHeight;
        private DataGridView dgvMembers;

        private ComboBox cboBodyMember;
        private DateTimePicker dtRecordDate;
        private NumericUpDown nudWeight;
        private NumericUpDown nudBodyFat;
        private NumericUpDown nudMuscle;
        private NumericUpDown nudVisceral;
        private NumericUpDown nudWaist;
        private NumericUpDown nudHip;
        private DataGridView dgvBodyRecords;

        private ComboBox cboDietMember;
        private ComboBox cboActivity;
        private ComboBox cboDietGoal;
        private TextBox txtDietResult;

        private ComboBox cboPayMember;
        private ComboBox cboPlan;
        private ComboBox cboPayMethod;
        private DateTimePicker dtPayDate;
        private NumericUpDown nudPayAmount;
        private TextBox txtPayNote;
        private DataGridView dgvPayments;

        public MainForm()
        {
            LoadAllData();

            Text = "FitPro Manager｜黑黃專業版健身房會員系統";
            MinimumSize = new Size(1450, 840);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Microsoft JhengHei UI", 10F, FontStyle.Regular);
            BackColor = PageBg;
            DoubleBuffered = true;

            BuildShell();
            Navigate("Home");
        }

        private void LoadAllData()
        {
            members = DataStore.LoadMembers();
            bodyRecords = DataStore.LoadBodyRecords();
            payments = DataStore.LoadPayments();
        }

        private void BuildShell()
        {
            navPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = NavBg,
                Padding = new Padding(18, 18, 18, 18)
            };
            Controls.Add(navPanel);

            var brand = new Label
            {
                Text = "FITPRO\nMANAGER",
                Dock = DockStyle.Top,
                Height = 86,
                ForeColor = NavActive,
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };
            navPanel.Controls.Add(brand);

            var caption = new Label
            {
                Text = "Gym Membership System",
                Dock = DockStyle.Top,
                Height = 28,
                ForeColor = Color.FromArgb(161, 161, 170),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft
            };
            navPanel.Controls.Add(caption);
            caption.BringToFront();

            var navWrap = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = NavBg,
                Padding = new Padding(0, 22, 0, 0)
            };
            navPanel.Controls.Add(navWrap);
            navWrap.BringToFront();

            AddNavButton(navWrap, "Home", "主畫面");
            AddNavButton(navWrap, "Members", "01 會員資料");
            AddNavButton(navWrap, "Body", "02 身體分析");
            AddNavButton(navWrap, "Progress", "03 成長軌跡");
            AddNavButton(navWrap, "Nutrition", "04 飲食規劃");
            AddNavButton(navWrap, "Payments", "05 付款與報表");

            var footer = new Label
            {
                Text = ".NET Framework 4.7.2\nWindows Forms App",
                Dock = DockStyle.Bottom,
                Height = 58,
                ForeColor = Color.FromArgb(161, 161, 170),
                Font = new Font("Segoe UI", 9F),
                TextAlign = ContentAlignment.BottomLeft
            };
            navPanel.Controls.Add(footer);
            footer.BringToFront();

            var mainPanel = new Panel { Dock = DockStyle.Fill, BackColor = PageBg };
            Controls.Add(mainPanel);
            mainPanel.BringToFront();

            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 98,
                BackColor = PageBg,
                Padding = new Padding(28, 20, 32, 14)
            };
            mainPanel.Controls.Add(headerPanel);

            lblTitle = new Label
            {
                Text = "主畫面",
                Location = new Point(28, 18),
                Size = new Size(620, 38),
                Font = new Font("Segoe UI", 23F, FontStyle.Bold),
                ForeColor = TextMain
            };
            headerPanel.Controls.Add(lblTitle);

            lblSubtitle = new Label
            {
                Text = "先選擇功能分類，畫面比較乾淨，不會全部擠在一起。",
                Location = new Point(30, 58),
                Size = new Size(780, 28),
                Font = new Font("Microsoft JhengHei UI", 9.5F),
                ForeColor = TextSub
            };
            headerPanel.Controls.Add(lblSubtitle);

            var topTools = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                Width = 430,
                Height = 52,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = PageBg,
                Padding = new Padding(0, 14, 0, 0)
            };
            headerPanel.Controls.Add(topTools);
            topTools.BringToFront();

            var selectLabel = new Label
            {
                Text = "目前會員",
                Width = 70,
                Height = 34,
                TextAlign = ContentAlignment.MiddleRight,
                ForeColor = TextSub,
                Font = new Font("Microsoft JhengHei UI", 9F, FontStyle.Bold)
            };
            topTools.Controls.Add(selectLabel);

            cboHeaderMember = new ComboBox
            {
                Width = 210,
                Height = 32,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Microsoft JhengHei UI", 10F),
                BackColor = InputBg,
                ForeColor = TextMain
            };
            cboHeaderMember.SelectedIndexChanged += delegate
            {
                if (!isBinding && (currentPage == "Body" || currentPage == "Progress" || currentPage == "Nutrition" || currentPage == "Payments"))
                    RebuildCurrentPage();
            };
            topTools.Controls.Add(cboHeaderMember);

            var reload = CreateButton("重新整理", Blue, 105);
            reload.Click += delegate { LoadAllData(); RefreshHeaderMemberCombo(); RebuildCurrentPage(); };
            topTools.Controls.Add(reload);

            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = PageBg,
                Padding = new Padding(28, 4, 32, 28),
                AutoScroll = true
            };
            mainPanel.Controls.Add(contentPanel);
            contentPanel.BringToFront();
        }

        private void AddNavButton(FlowLayoutPanel wrap, string page, string text)
        {
            var button = new Button
            {
                Text = "  " + text,
                Tag = page,
                Width = 214,
                Height = 46,
                Margin = new Padding(0, 0, 0, 10),
                FlatStyle = FlatStyle.Flat,
                BackColor = NavBg,
                ForeColor = Color.FromArgb(228, 228, 231),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Microsoft JhengHei UI", 10.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = NavHover;
            button.Click += delegate { Navigate(page); };
            navButtons.Add(button);
            wrap.Controls.Add(button);
        }

        private void Navigate(string page)
        {
            currentPage = page;
            RefreshHeaderMemberCombo();

            foreach (var btn in navButtons)
            {
                bool active = (string)btn.Tag == page;
                btn.BackColor = active ? NavActive : NavBg;
                btn.ForeColor = active ? Color.Black : Color.FromArgb(228, 228, 231);
            }

            RebuildCurrentPage();
        }

        private void RebuildCurrentPage()
        {
            contentPanel.Controls.Clear();
            contentPanel.AutoScrollPosition = new Point(0, 0);

            if (currentPage == "Home") BuildHomePage();
            else if (currentPage == "Members") BuildMembersPage();
            else if (currentPage == "Body") BuildBodyPage();
            else if (currentPage == "Progress") BuildProgressPage();
            else if (currentPage == "Nutrition") BuildNutritionPage();
            else if (currentPage == "Payments") BuildPaymentsPage();
        }

        private void SetHeader(string title, string subtitle)
        {
            lblTitle.Text = title;
            lblSubtitle.Text = subtitle;
        }

        private void BuildHomePage()
        {
            SetHeader("主畫面", "請先從下方功能卡片進入各功能區，讓操作更清楚。");

            var canvas = CreateCanvas(1120, 960);

            var hero = CreateCard(canvas, 0, 0, 1120, 165);
            hero.BackColor = Color.FromArgb(0, 0, 0);
            AddLabel(hero, "FITPRO MEMBER CENTER", 34, 24, 620, 42, NavActive, 24F, true);
            AddLabel(hero, "健身房會員管理、身體組成追蹤、飲食規劃與會籍報表整合系統", 36, 70, 780, 26, Color.FromArgb(203, 213, 225), 11F, false);
            AddLabel(hero, "請從下方功能卡片進入各功能區，依需求管理會員資料、身體數據、飲食與付款報表。", 36, 106, 870, 24, Color.FromArgb(148, 163, 184), 9.5F, false);
            var heroButton = CreateButton("開始管理會員", Teal, 150);
            heroButton.Location = new Point(915, 57);
            heroButton.Click += delegate { Navigate("Members"); };
            hero.Controls.Add(heroButton);

            AddMetricCard(canvas, 0, 188, 255, 112, "會員總數", members.Count.ToString(), "目前系統已建立會員", Blue);
            AddMetricCard(canvas, 288, 188, 255, 112, "有效會籍", members.Count(m => m.Status == "有效").ToString(), "到期日仍有效", Green);
            AddMetricCard(canvas, 576, 188, 255, 112, "即將到期", members.Count(m => m.Status == "有效" && m.RemainingDays <= 14).ToString(), "14 天內需要提醒", Orange);
            AddMetricCard(canvas, 864, 188, 255, 112, "身體紀錄", bodyRecords.Count.ToString(), "可用於成長軌跡", Purple);

            CreateCategoryCard(canvas, 0, 330, 540, 150, "01 會員資料", "新增、修改、刪除、搜尋會員。適合放會員基本資料、教練、到期日與目標。", "Members", Blue);
            CreateCategoryCard(canvas, 580, 330, 540, 150, "02 身體分析", "輸入體重、體脂、肌肉量、腰圍、臀圍等數據，建立類似 TANITA / InBody 的紀錄。", "Body", Teal);
            CreateCategoryCard(canvas, 0, 510, 540, 150, "03 成長軌跡", "用折線圖呈現體重、體脂率、肌肉量變化，讓會員看得到訓練成果。", "Progress", Green);
            CreateCategoryCard(canvas, 580, 510, 540, 150, "04 飲食規劃", "依照性別、年齡、身高、最新體重與目標計算 BMR、TDEE 和三大營養素。", "Nutrition", Orange);
            CreateCategoryCard(canvas, 0, 690, 1120, 150, "05 付款與報表", "管理月卡、季卡、半年卡、年卡付款，並匯出會員摘要、身體分析、飲食建議等文字報表。", "Payments", Purple);
        }

        private void CreateCategoryCard(Control parent, int x, int y, int w, int h, string title, string desc, string page, Color color)
        {
            var card = CreateCard(parent, x, y, w, h);
            var strip = new Panel { Location = new Point(0, 0), Size = new Size(8, h), BackColor = color };
            card.Controls.Add(strip);
            AddLabel(card, title, 28, 20, w - 190, 32, TextMain, 17F, true);
            AddLabel(card, desc, 30, 62, w - 205, 52, TextSub, 10F, false);
            var go = CreateButton("進入", color, 110);
            go.Location = new Point(w - 142, 55);
            go.Click += delegate { Navigate(page); };
            card.Controls.Add(go);
        }

        private void BuildMembersPage()
        {
            SetHeader("01 會員資料", "黑黃專業版會員資料管理：可新增會員照片、修改基本資料、搜尋與管理會籍狀態。 ");
            editingMemberId = -1;
            currentPhotoPath = string.Empty;

            var canvas = CreateCanvas(1120, 820);
            var formCard = CreateCard(canvas, 0, 0, 340, 780);
            AddLabel(formCard, "會員表單", 22, 20, 280, 28, TextMain, 15F, true);
            AddLabel(formCard, "會員照片 + 基本資料", 22, 50, 260, 24, TextSub, 9.5F, false);

            picMemberPhoto = new PictureBox
            {
                Location = new Point(22, 88),
                Size = new Size(112, 112),
                BackColor = Color.FromArgb(0, 0, 0),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            formCard.Controls.Add(picMemberPhoto);
            SetMemberPhotoPreview(string.Empty, "");

            var choosePhoto = CreateButton("選擇照片", NavActive, 160);
            choosePhoto.Location = new Point(154, 96);
            choosePhoto.Click += delegate { ChooseMemberPhoto(); };
            formCard.Controls.Add(choosePhoto);

            var removePhoto = CreateButton("移除照片", Color.FromArgb(82, 82, 91), 160);
            removePhoto.Location = new Point(154, 144);
            removePhoto.Click += delegate
            {
                currentPhotoPath = string.Empty;
                SetMemberPhotoPreview(string.Empty, txtMemberName == null ? "" : txtMemberName.Text);
            };
            formCard.Controls.Add(removePhoto);

            AddLabel(formCard, "支援 JPG / PNG / BMP，儲存會員時會複製到 Data\\Photos。", 154, 190, 160, 44, TextSub, 8.5F, false);

            txtMemberName = CreateTextBox(formCard, "姓名", 22, 258, 296);
            txtMemberPhone = CreateTextBox(formCard, "電話", 22, 322, 296);
            cboMemberGender = CreateCombo(formCard, "性別", 22, 386, 140, new[] { "男", "女", "其他" });
            cboMemberGoal = CreateCombo(formCard, "目標", 178, 386, 140, new[] { "減脂", "增肌", "維持健康", "體態改善" });
            nudMemberHeight = CreateNumber(formCard, "身高 cm", 22, 450, 140, 100, 230, 1);
            txtMemberTrainer = CreateTextBox(formCard, "教練", 178, 450, 140);
            dtMemberBirthday = CreateDatePicker(formCard, "生日", 22, 514, 296);
            dtMemberJoin = CreateDatePicker(formCard, "入會日期", 22, 578, 296);
            dtMemberExpire = CreateDatePicker(formCard, "到期日期", 22, 642, 296);

            var add = CreateButton("新增會員", NavActive, 140);
            add.Location = new Point(22, 704);
            add.Click += delegate { AddOrUpdateMember(false); };
            formCard.Controls.Add(add);

            var update = CreateButton("修改會員", Color.FromArgb(217, 119, 6), 140);
            update.Location = new Point(178, 704);
            update.Click += delegate { AddOrUpdateMember(true); };
            formCard.Controls.Add(update);

            var clear = CreateButton("清空", Color.FromArgb(82, 82, 91), 140);
            clear.Location = new Point(22, 748);
            clear.Click += delegate { ClearMemberForm(); };
            formCard.Controls.Add(clear);

            var delete = CreateButton("刪除", Red, 140);
            delete.Location = new Point(178, 748);
            delete.Click += delegate { DeleteSelectedMember(); };
            formCard.Controls.Add(delete);

            var listCard = CreateCard(canvas, 370, 0, 750, 780);
            AddLabel(listCard, "會員列表", 22, 20, 300, 28, TextMain, 15F, true);
            AddLabel(listCard, "點選表格列即可載入左側表單，並顯示會員照片。", 22, 50, 420, 24, TextSub, 9.5F, false);
            txtMemberSearch = new TextBox
            {
                Location = new Point(448, 24),
                Width = 200,
                Height = 30,
                Font = new Font("Microsoft JhengHei UI", 10F),
                BackColor = InputBg,
                ForeColor = TextMain,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtMemberSearch.TextChanged += delegate { UpdateMemberGrid(); };
            listCard.Controls.Add(txtMemberSearch);
            var searchLabel = new Label { Text = "搜尋", Location = new Point(400, 28), Size = new Size(42, 24), ForeColor = TextSub, TextAlign = ContentAlignment.MiddleRight };
            listCard.Controls.Add(searchLabel);

            dgvMembers = CreateGrid(listCard, 22, 82, 706, 660);
            dgvMembers.CellClick += delegate (object sender, DataGridViewCellEventArgs e)
            {
                if (e.RowIndex >= 0 && dgvMembers.Rows[e.RowIndex].Cells["Id"].Value != null)
                {
                    int id = Convert.ToInt32(dgvMembers.Rows[e.RowIndex].Cells["Id"].Value);
                    LoadMemberIntoForm(id);
                }
            };
            UpdateMemberGrid();
        }

        private void BuildBodyPage()
        {
            SetHeader("02 身體分析", "此頁專門輸入與查看單次身體組成資料，像量 InBody 後登錄數據。 ");

            var canvas = CreateCanvas(1120, 760);
            var inputCard = CreateCard(canvas, 0, 0, 340, 720);
            AddLabel(inputCard, "新增身體紀錄", 22, 20, 280, 28, TextMain, 15F, true);
            AddLabel(inputCard, "選會員後輸入本次量測數據", 22, 50, 260, 24, TextSub, 9.5F, false);

            cboBodyMember = CreateMemberCombo(inputCard, "會員", 22, 92, 296);
            dtRecordDate = CreateDatePicker(inputCard, "量測日期", 22, 156, 296);
            nudWeight = CreateNumber(inputCard, "體重 kg", 22, 220, 140, 20, 200, 1);
            nudBodyFat = CreateNumber(inputCard, "體脂率 %", 178, 220, 140, 1, 70, 1);
            nudMuscle = CreateNumber(inputCard, "肌肉量 kg", 22, 284, 140, 5, 120, 1);
            nudVisceral = CreateNumber(inputCard, "內臟脂肪", 178, 284, 140, 1, 30, 0);
            nudWaist = CreateNumber(inputCard, "腰圍 cm", 22, 348, 140, 30, 180, 1);
            nudHip = CreateNumber(inputCard, "臀圍 cm", 178, 348, 140, 30, 180, 1);

            var save = CreateButton("儲存身體紀錄", Teal, 296);
            save.Location = new Point(22, 424);
            save.Click += delegate { SaveBodyRecord(); };
            inputCard.Controls.Add(save);

            var hint = new Label
            {
                Text = "建議每次會員完成量測後都新增一筆。\n成長軌跡頁會自動抓這裡的資料畫圖。",
                Location = new Point(22, 482),
                Size = new Size(296, 74),
                ForeColor = TextSub,
                Font = new Font("Microsoft JhengHei UI", 9.5F)
            };
            inputCard.Controls.Add(hint);

            var summary = CreateCard(canvas, 370, 0, 750, 245);
            AddLabel(summary, "最新身體組成摘要", 22, 20, 300, 28, TextMain, 15F, true);
            AddLabel(summary, "依目前選擇會員的最新一筆紀錄顯示", 22, 50, 360, 24, TextSub, 9.5F, false);
            BuildLatestBodySummary(summary);

            var records = CreateCard(canvas, 370, 275, 750, 445);
            AddLabel(records, "身體紀錄列表", 22, 20, 300, 28, TextMain, 15F, true);
            dgvBodyRecords = CreateGrid(records, 22, 68, 706, 345);
            UpdateBodyGrid();
        }

        private void BuildProgressPage()
        {
            SetHeader("03 成長軌跡", "此頁只看趨勢圖：體重、體脂、肌肉量，可以快速查看會員訓練成果。 ");

            var canvas = CreateCanvas(1120, 740);
            var chartCard = CreateCard(canvas, 0, 0, 1120, 480);
            AddLabel(chartCard, "會員身體數據趨勢圖", 22, 20, 420, 28, TextMain, 15F, true);
            AddLabel(chartCard, "折線圖會依照身體分析頁的紀錄自動更新", 22, 50, 420, 24, TextSub, 9.5F, false);
            var chart = CreateProgressChart(GetSelectedHeaderMemberId());
            chart.Location = new Point(22, 82);
            chart.Size = new Size(1076, 370);
            chartCard.Controls.Add(chart);

            int memberId = GetSelectedHeaderMemberId();
            var latest = GetLatestRecord(memberId);
            var earliest = bodyRecords.Where(r => r.MemberId == memberId).OrderBy(r => r.RecordDate).FirstOrDefault();
            if (latest != null && earliest != null)
            {
                AddMetricCard(canvas, 0, 510, 255, 112, "目前體重", latest.WeightKg.ToString("0.0") + " kg", DeltaText(latest.WeightKg - earliest.WeightKg, "kg"), Blue);
                AddMetricCard(canvas, 288, 510, 255, 112, "目前體脂", latest.BodyFatPct.ToString("0.0") + " %", DeltaText(latest.BodyFatPct - earliest.BodyFatPct, "%"), Orange);
                AddMetricCard(canvas, 576, 510, 255, 112, "目前肌肉", latest.MuscleKg.ToString("0.0") + " kg", DeltaText(latest.MuscleKg - earliest.MuscleKg, "kg"), Green);
                AddMetricCard(canvas, 864, 510, 255, 112, "BMI", CalculateBmi(GetMember(memberId), latest).ToString("0.0"), "依最新體重與身高計算", Purple);
            }
            else
            {
                AddMetricCard(canvas, 0, 510, 1120, 112, "尚無資料", "請先新增身體紀錄", "到「02 身體分析」輸入量測資料後，這裡就會出現成長軌跡。", Orange);
            }
        }

        private void BuildNutritionPage()
        {
            SetHeader("04 飲食規劃", "此頁專門做飲食建議，不和會員資料或身體分析混在一起。 ");

            var canvas = CreateCanvas(1120, 740);
            var input = CreateCard(canvas, 0, 0, 340, 700);
            AddLabel(input, "飲食規劃設定", 22, 20, 280, 28, TextMain, 15F, true);
            AddLabel(input, "依會員最新身體紀錄估算", 22, 50, 260, 24, TextSub, 9.5F, false);
            cboDietMember = CreateMemberCombo(input, "會員", 22, 92, 296);
            cboActivity = CreateCombo(input, "活動量", 22, 156, 296, new[] { "久坐｜1.20", "輕量運動｜1.375", "中等運動｜1.55", "高強度運動｜1.725" });
            cboDietGoal = CreateCombo(input, "飲食目標", 22, 220, 296, new[] { "減脂", "增肌", "維持" });

            var generate = CreateButton("產生飲食規劃", Teal, 296);
            generate.Location = new Point(22, 294);
            generate.Click += delegate { GenerateDietPlan(); };
            input.Controls.Add(generate);

            var save = CreateButton("儲存 TXT", Blue, 296);
            save.Location = new Point(22, 346);
            save.Click += delegate { SaveDietPlan(); };
            input.Controls.Add(save);

            var rules = new Label
            {
                Text = "計算邏輯：\n1. 用 Mifflin-St Jeor 估 BMR\n2. BMR × 活動係數 = TDEE\n3. 減脂 -500 kcal，增肌 +300 kcal\n4. 再換算蛋白質、脂肪、碳水",
                Location = new Point(22, 430),
                Size = new Size(296, 150),
                ForeColor = TextSub,
                Font = new Font("Microsoft JhengHei UI", 9.5F)
            };
            input.Controls.Add(rules);

            var result = CreateCard(canvas, 370, 0, 750, 700);
            AddLabel(result, "規劃結果", 22, 20, 300, 28, TextMain, 15F, true);
            AddLabel(result, "含熱量、三大營養素與一日菜單範例", 22, 50, 400, 24, TextSub, 9.5F, false);
            txtDietResult = new TextBox
            {
                Location = new Point(22, 86),
                Size = new Size(706, 580),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Consolas", 10.5F),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = InputBg,
                ForeColor = TextMain
            };
            result.Controls.Add(txtDietResult);
            GenerateDietPlan();
        }

        private void BuildPaymentsPage()
        {
            SetHeader("05 付款與報表", "此頁集中處理會籍付款、延長到期日，以及期末報告可展示的匯出功能。 ");

            var canvas = CreateCanvas(1120, 760);
            var payCard = CreateCard(canvas, 0, 0, 340, 720);
            AddLabel(payCard, "新增付款", 22, 20, 280, 28, TextMain, 15F, true);
            AddLabel(payCard, "儲存付款後會自動延長會員到期日", 22, 50, 286, 24, TextSub, 9.5F, false);
            cboPayMember = CreateMemberCombo(payCard, "會員", 22, 92, 296);
            cboPlan = CreateCombo(payCard, "方案", 22, 156, 296, new[] { "單次入場｜1｜200", "月卡｜30｜1500", "季卡｜90｜4000", "半年卡｜180｜7200", "年卡｜365｜12000" });
            cboPayMethod = CreateCombo(payCard, "付款方式", 22, 220, 296, new[] { "現金", "信用卡", "轉帳", "Line Pay" });
            dtPayDate = CreateDatePicker(payCard, "付款日期", 22, 284, 296);
            nudPayAmount = CreateNumber(payCard, "金額 NT$", 22, 348, 296, 0, 100000, 0);
            txtPayNote = CreateTextBox(payCard, "備註", 22, 412, 296);
            cboPlan.SelectedIndexChanged += delegate { FillPlanAmount(); };
            FillPlanAmount();

            var save = CreateButton("儲存付款並更新會籍", Teal, 296);
            save.Location = new Point(22, 490);
            save.Click += delegate { SavePayment(); };
            payCard.Controls.Add(save);

            var report1 = CreateButton("匯出會員摘要", Blue, 296);
            report1.Location = new Point(22, 548);
            report1.Click += delegate { ExportMemberSummary(); };
            payCard.Controls.Add(report1);

            var report2 = CreateButton("匯出身體分析報告", Purple, 296);
            report2.Location = new Point(22, 606);
            report2.Click += delegate { ExportBodyReport(); };
            payCard.Controls.Add(report2);

            var listCard = CreateCard(canvas, 370, 0, 750, 720);
            AddLabel(listCard, "付款紀錄", 22, 20, 300, 28, TextMain, 15F, true);
            AddLabel(listCard, "可作為系統讀寫檔與營運管理功能展示", 22, 50, 420, 24, TextSub, 9.5F, false);
            dgvPayments = CreateGrid(listCard, 22, 86, 706, 590);
            UpdatePaymentGrid();
        }

        private void AddOrUpdateMember(bool update)
        {
            if (string.IsNullOrWhiteSpace(txtMemberName.Text))
            {
                MessageBox.Show("請輸入會員姓名。", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Member m;
            if (update)
            {
                if (editingMemberId <= 0)
                {
                    MessageBox.Show("請先從右側表格選擇要修改的會員。", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                m = members.FirstOrDefault(x => x.Id == editingMemberId);
                if (m == null) return;
            }
            else
            {
                m = new Member { Id = members.Count == 0 ? 1 : members.Max(x => x.Id) + 1 };
                members.Add(m);
            }

            m.Name = txtMemberName.Text.Trim();
            m.Phone = txtMemberPhone.Text.Trim();
            m.Gender = cboMemberGender.Text;
            m.Goal = cboMemberGoal.Text;
            m.HeightCm = (double)nudMemberHeight.Value;
            m.Trainer = txtMemberTrainer.Text.Trim();
            m.Birthday = dtMemberBirthday.Value.Date;
            m.JoinDate = dtMemberJoin.Value.Date;
            m.ExpireDate = dtMemberExpire.Value.Date;
            m.PhotoPath = string.IsNullOrWhiteSpace(currentPhotoPath) ? string.Empty : DataStore.SaveMemberPhoto(m.Id, currentPhotoPath);
            currentPhotoPath = m.PhotoPath;

            DataStore.SaveMembers(members);
            RefreshHeaderMemberCombo();
            UpdateMemberGrid();
            MessageBox.Show(update ? "會員資料已修改。" : "會員已新增。", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DeleteSelectedMember()
        {
            if (editingMemberId <= 0)
            {
                MessageBox.Show("請先選擇要刪除的會員。", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var m = members.FirstOrDefault(x => x.Id == editingMemberId);
            if (m == null) return;
            var result = MessageBox.Show("確定要刪除「" + m.Name + "」嗎？相關身體紀錄與付款紀錄也會一起移除。", "確認刪除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            members.RemoveAll(x => x.Id == editingMemberId);
            bodyRecords.RemoveAll(x => x.MemberId == editingMemberId);
            payments.RemoveAll(x => x.MemberId == editingMemberId);
            DataStore.SaveMembers(members);
            DataStore.SaveBodyRecords(bodyRecords);
            DataStore.SavePayments(payments);
            editingMemberId = -1;
            ClearMemberForm();
            RefreshHeaderMemberCombo();
            UpdateMemberGrid();
            MessageBox.Show("已刪除會員。", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ClearMemberForm()
        {
            editingMemberId = -1;
            txtMemberName.Text = "";
            txtMemberPhone.Text = "";
            txtMemberTrainer.Text = "";
            cboMemberGender.SelectedIndex = 0;
            cboMemberGoal.SelectedIndex = 0;
            nudMemberHeight.Value = 170;
            dtMemberBirthday.Value = DateTime.Today.AddYears(-20);
            dtMemberJoin.Value = DateTime.Today;
            dtMemberExpire.Value = DateTime.Today.AddMonths(1);
            currentPhotoPath = string.Empty;
            SetMemberPhotoPreview(string.Empty, "");
        }

        private void LoadMemberIntoForm(int id)
        {
            var m = members.FirstOrDefault(x => x.Id == id);
            if (m == null) return;
            editingMemberId = m.Id;
            txtMemberName.Text = m.Name;
            txtMemberPhone.Text = m.Phone;
            txtMemberTrainer.Text = m.Trainer;
            cboMemberGender.Text = m.Gender;
            cboMemberGoal.Text = m.Goal;
            nudMemberHeight.Value = ClampDecimal((decimal)m.HeightCm, nudMemberHeight.Minimum, nudMemberHeight.Maximum);
            dtMemberBirthday.Value = SafeDate(m.Birthday);
            dtMemberJoin.Value = SafeDate(m.JoinDate);
            dtMemberExpire.Value = SafeDate(m.ExpireDate);
            currentPhotoPath = m.PhotoPath ?? string.Empty;
            SetMemberPhotoPreview(currentPhotoPath, m.Name);
        }

        private void UpdateMemberGrid()
        {
            if (dgvMembers == null) return;
            string q = txtMemberSearch == null ? "" : txtMemberSearch.Text.Trim();
            var data = members
                .Where(m => string.IsNullOrWhiteSpace(q) || m.Name.Contains(q) || m.Phone.Contains(q) || m.Id.ToString().Contains(q))
                .OrderBy(m => m.Id)
                .Select(m => new
                {
                    Id = m.Id,
                    照片 = string.IsNullOrWhiteSpace(m.PhotoPath) ? "無" : "有",
                    姓名 = m.Name,
                    電話 = m.Phone,
                    性別 = m.Gender,
                    年齡 = m.Age,
                    身高 = m.HeightCm,
                    目標 = m.Goal,
                    教練 = m.Trainer,
                    到期日 = m.ExpireDate.ToString("yyyy-MM-dd"),
                    狀態 = m.Status,
                    剩餘天數 = m.RemainingDays
                })
                .ToList();
            dgvMembers.DataSource = data;
            if (dgvMembers.Columns.Contains("Id")) dgvMembers.Columns["Id"].Width = 54;
            if (dgvMembers.Columns.Contains("照片")) dgvMembers.Columns["照片"].Width = 54;
        }

        private void SaveBodyRecord()
        {
            int memberId = GetComboMemberId(cboBodyMember);
            if (memberId <= 0)
            {
                MessageBox.Show("請先建立或選擇會員。", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var record = new BodyRecord
            {
                Id = bodyRecords.Count == 0 ? 1 : bodyRecords.Max(x => x.Id) + 1,
                MemberId = memberId,
                RecordDate = dtRecordDate.Value.Date,
                WeightKg = (double)nudWeight.Value,
                BodyFatPct = (double)nudBodyFat.Value,
                MuscleKg = (double)nudMuscle.Value,
                VisceralFat = (double)nudVisceral.Value,
                WaistCm = (double)nudWaist.Value,
                HipCm = (double)nudHip.Value
            };
            bodyRecords.Add(record);
            DataStore.SaveBodyRecords(bodyRecords);
            MessageBox.Show("身體紀錄已儲存，成長軌跡會自動更新。", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RebuildCurrentPage();
        }

        private void UpdateBodyGrid()
        {
            if (dgvBodyRecords == null) return;
            int memberId = GetSelectedHeaderMemberId();
            var data = bodyRecords
                .Where(r => memberId <= 0 || r.MemberId == memberId)
                .OrderByDescending(r => r.RecordDate)
                .Select(r => new
                {
                    日期 = r.RecordDate.ToString("yyyy-MM-dd"),
                    會員 = GetMemberName(r.MemberId),
                    體重 = r.WeightKg.ToString("0.0"),
                    體脂率 = r.BodyFatPct.ToString("0.0"),
                    肌肉量 = r.MuscleKg.ToString("0.0"),
                    內臟脂肪 = r.VisceralFat.ToString("0"),
                    腰圍 = r.WaistCm.ToString("0.0"),
                    臀圍 = r.HipCm.ToString("0.0")
                })
                .ToList();
            dgvBodyRecords.DataSource = data;
        }

        private void BuildLatestBodySummary(Control parent)
        {
            int memberId = GetSelectedHeaderMemberId();
            var m = GetMember(memberId);
            var latest = GetLatestRecord(memberId);
            if (m == null || latest == null)
            {
                AddLabel(parent, "目前會員尚無身體紀錄。請先在左側新增量測數據。", 24, 100, 600, 32, TextSub, 11F, false);
                return;
            }

            AddSmallMetric(parent, 22, 92, 130, 92, "體重", latest.WeightKg.ToString("0.0") + " kg", Blue);
            AddSmallMetric(parent, 166, 92, 130, 92, "體脂", latest.BodyFatPct.ToString("0.0") + " %", Orange);
            AddSmallMetric(parent, 310, 92, 130, 92, "肌肉", latest.MuscleKg.ToString("0.0") + " kg", Green);
            AddSmallMetric(parent, 454, 92, 130, 92, "BMI", CalculateBmi(m, latest).ToString("0.0"), Purple);
            AddSmallMetric(parent, 598, 92, 130, 92, "內臟脂肪", latest.VisceralFat.ToString("0"), Red);
        }

        private void GenerateDietPlan()
        {
            if (txtDietResult == null) return;
            int memberId = GetComboMemberId(cboDietMember);
            var member = GetMember(memberId);
            var latest = GetLatestRecord(memberId);
            if (member == null)
            {
                txtDietResult.Text = "請先建立會員資料。";
                return;
            }

            double weight = latest != null ? latest.WeightKg : 70.0;
            double height = member.HeightCm <= 0 ? 170.0 : member.HeightCm;
            int age = Math.Max(member.Age, 18);
            bool male = member.Gender == "男";
            double bmr = 10 * weight + 6.25 * height - 5 * age + (male ? 5 : -161);
            double factor = ParseActivityFactor(cboActivity == null ? "" : cboActivity.Text);
            double tdee = bmr * factor;
            string goal = cboDietGoal == null ? "維持" : cboDietGoal.Text;
            double calories = tdee;
            if (goal == "減脂") calories -= 500;
            else if (goal == "增肌") calories += 300;
            calories = Math.Max(1200, calories);

            double protein = weight * (goal == "增肌" ? 2.0 : 1.8);
            double fat = weight * 0.8;
            double carbs = Math.Max(80, (calories - protein * 4 - fat * 9) / 4);

            var sb = new StringBuilder();
            sb.AppendLine("FITPRO 飲食規劃建議");
            sb.AppendLine("====================================");
            sb.AppendLine("會員：" + member.Name + "    目標：" + goal);
            sb.AppendLine("身高：" + height.ToString("0") + " cm    體重：" + weight.ToString("0.0") + " kg    年齡：" + age);
            sb.AppendLine();
            sb.AppendLine("BMR 基礎代謝：" + bmr.ToString("0") + " kcal");
            sb.AppendLine("TDEE 每日總消耗：" + tdee.ToString("0") + " kcal");
            sb.AppendLine("建議每日熱量：" + calories.ToString("0") + " kcal");
            sb.AppendLine();
            sb.AppendLine("三大營養素建議");
            sb.AppendLine("蛋白質：" + protein.ToString("0") + " g / day");
            sb.AppendLine("脂肪：" + fat.ToString("0") + " g / day");
            sb.AppendLine("碳水：" + carbs.ToString("0") + " g / day");
            sb.AppendLine();
            sb.AppendLine("一日菜單範例");
            sb.AppendLine("早餐：燕麥 + 雞蛋 + 無糖豆漿");
            sb.AppendLine("午餐：雞胸肉 / 牛肉 + 白飯 + 青菜");
            sb.AppendLine("點心：希臘優格或乳清蛋白");
            sb.AppendLine("晚餐：魚肉 / 豆腐 + 地瓜 + 大量蔬菜");
            sb.AppendLine();
            sb.AppendLine("備註：此為健身房系統估算建議，實際飲食仍需依個人健康狀況調整。");
            txtDietResult.Text = sb.ToString();
        }

        private void SaveDietPlan()
        {
            if (string.IsNullOrWhiteSpace(txtDietResult.Text)) GenerateDietPlan();
            int memberId = GetComboMemberId(cboDietMember);
            string file = DataStore.SaveDietPlan(memberId, txtDietResult.Text);
            MessageBox.Show("飲食規劃已儲存：\n" + file, "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FillPlanAmount()
        {
            if (cboPlan == null || nudPayAmount == null || cboPlan.SelectedItem == null) return;
            string[] parts = cboPlan.Text.Split('｜');
            if (parts.Length >= 3)
            {
                decimal price;
                if (decimal.TryParse(parts[2], out price)) nudPayAmount.Value = ClampDecimal(price, nudPayAmount.Minimum, nudPayAmount.Maximum);
            }
        }

        private void SavePayment()
        {
            int memberId = GetComboMemberId(cboPayMember);
            var member = GetMember(memberId);
            if (member == null)
            {
                MessageBox.Show("請先選擇會員。", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string[] parts = cboPlan.Text.Split('｜');
            string planName = parts.Length > 0 ? parts[0] : "月卡";
            int days = 30;
            if (parts.Length > 1) int.TryParse(parts[1], out days);

            var pay = new PaymentRecord
            {
                Id = payments.Count == 0 ? 1 : payments.Max(x => x.Id) + 1,
                MemberId = memberId,
                PayDate = dtPayDate.Value.Date,
                PlanName = planName,
                Days = days,
                Amount = nudPayAmount.Value,
                Method = cboPayMethod.Text,
                Note = txtPayNote.Text.Trim()
            };
            payments.Add(pay);
            member.ExpireDate = member.ExpireDate.Date < DateTime.Today ? DateTime.Today.AddDays(days) : member.ExpireDate.AddDays(days);

            DataStore.SavePayments(payments);
            DataStore.SaveMembers(members);
            RefreshHeaderMemberCombo();
            UpdatePaymentGrid();
            MessageBox.Show("付款已儲存，會員到期日已更新。", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdatePaymentGrid()
        {
            if (dgvPayments == null) return;
            int memberId = GetSelectedHeaderMemberId();
            dgvPayments.DataSource = payments
                .Where(p => memberId <= 0 || p.MemberId == memberId)
                .OrderByDescending(p => p.PayDate)
                .Select(p => new
                {
                    日期 = p.PayDate.ToString("yyyy-MM-dd"),
                    會員 = GetMemberName(p.MemberId),
                    方案 = p.PlanName,
                    天數 = p.Days,
                    金額 = "NT$ " + p.Amount.ToString("N0"),
                    付款方式 = p.Method,
                    備註 = p.Note
                })
                .ToList();
        }

        private void ExportMemberSummary()
        {
            var sb = new StringBuilder();
            sb.AppendLine("FITPRO 會員摘要報表");
            sb.AppendLine("匯出時間：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            sb.AppendLine("====================================");
            foreach (var m in members.OrderBy(x => x.Id))
            {
                sb.AppendLine(string.Format("#{0:000} {1}｜{2}｜{3}｜到期：{4:yyyy-MM-dd}｜狀態：{5}", m.Id, m.Name, m.Phone, m.Goal, m.ExpireDate, m.Status));
            }
            string file = DataStore.ExportReport("member_summary_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt", sb.ToString());
            MessageBox.Show("會員摘要已匯出：\n" + file, "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ExportBodyReport()
        {
            int memberId = GetSelectedHeaderMemberId();
            var m = GetMember(memberId);
            if (m == null)
            {
                MessageBox.Show("請先選擇會員。", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var records = bodyRecords.Where(r => r.MemberId == memberId).OrderBy(r => r.RecordDate).ToList();
            var sb = new StringBuilder();
            sb.AppendLine("FITPRO 身體分析報告");
            sb.AppendLine("會員：" + m.Name);
            sb.AppendLine("匯出時間：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            sb.AppendLine("====================================");
            foreach (var r in records)
            {
                sb.AppendLine(string.Format("{0:yyyy-MM-dd}｜體重 {1:0.0} kg｜體脂 {2:0.0}%｜肌肉 {3:0.0} kg｜腰圍 {4:0.0} cm", r.RecordDate, r.WeightKg, r.BodyFatPct, r.MuscleKg, r.WaistCm));
            }
            string file = DataStore.ExportReport("body_report_member_" + memberId + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt", sb.ToString());
            MessageBox.Show("身體分析報告已匯出：\n" + file, "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void ChooseMemberPhoto()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "選擇會員照片";
                dialog.Filter = "圖片檔案|*.jpg;*.jpeg;*.png;*.bmp|所有檔案|*.*";
                dialog.Multiselect = false;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    currentPhotoPath = dialog.FileName;
                    SetMemberPhotoPreview(currentPhotoPath, txtMemberName == null ? "" : txtMemberName.Text);
                }
            }
        }

        private void SetMemberPhotoPreview(string path, string memberName)
        {
            if (picMemberPhoto == null) return;
            if (picMemberPhoto.Image != null)
            {
                var old = picMemberPhoto.Image;
                picMemberPhoto.Image = null;
                old.Dispose();
            }

            if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
            {
                try
                {
                    using (var original = Image.FromFile(path))
                    {
                        picMemberPhoto.Image = new Bitmap(original);
                    }
                    return;
                }
                catch
                {
                    // 圖片檔損壞或格式不支援時，改顯示預設頭像。
                }
            }

            picMemberPhoto.Image = CreateAvatar(memberName);
        }

        private Bitmap CreateAvatar(string memberName)
        {
            var bmp = new Bitmap(112, 112);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.FromArgb(0, 0, 0));
                using (var ringPen = new Pen(NavActive, 4))
                using (var fillBrush = new SolidBrush(Color.FromArgb(39, 39, 42)))
                using (var textBrush = new SolidBrush(NavActive))
                using (var font = new Font("Segoe UI", 24F, FontStyle.Bold))
                using (var smallFont = new Font("Microsoft JhengHei UI", 8.5F, FontStyle.Bold))
                {
                    g.FillEllipse(fillBrush, 14, 10, 84, 84);
                    g.DrawEllipse(ringPen, 14, 10, 84, 84);
                    string initials = "PHOTO";
                    if (!string.IsNullOrWhiteSpace(memberName))
                    {
                        initials = memberName.Trim().Length >= 2 ? memberName.Trim().Substring(0, 2) : memberName.Trim();
                    }
                    var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    g.DrawString(initials, font, textBrush, new RectangleF(14, 16, 84, 62), format);
                    g.DrawString("MEMBER", smallFont, textBrush, new RectangleF(0, 84, 112, 20), format);
                }
            }
            return bmp;
        }

        private Panel CreateCanvas(int width, int height)
        {
            var panel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(width, height),
                BackColor = PageBg
            };
            contentPanel.Controls.Add(panel);
            return panel;
        }

        private Panel CreateCard(Control parent, int x, int y, int w, int h)
        {
            var card = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(w, h),
                BackColor = CardBg,
                BorderStyle = BorderStyle.FixedSingle
            };
            parent.Controls.Add(card);
            return card;
        }

        private void AddMetricCard(Control parent, int x, int y, int w, int h, string title, string value, string note, Color accent)
        {
            var card = CreateCard(parent, x, y, w, h);
            var strip = new Panel { Location = new Point(0, 0), Size = new Size(6, h), BackColor = accent };
            card.Controls.Add(strip);
            AddLabel(card, title, 22, 18, w - 44, 24, TextSub, 9.5F, true);
            AddLabel(card, value, 22, 42, w - 44, 38, TextMain, 20F, true);
            AddLabel(card, note, 22, 82, w - 44, 22, TextSub, 8.8F, false);
        }

        private void AddSmallMetric(Control parent, int x, int y, int w, int h, string title, string value, Color accent)
        {
            var panel = new Panel { Location = new Point(x, y), Size = new Size(w, h), BackColor = Color.FromArgb(31, 31, 31), BorderStyle = BorderStyle.FixedSingle };
            parent.Controls.Add(panel);
            AddLabel(panel, title, 10, 12, w - 20, 22, TextSub, 9F, true);
            AddLabel(panel, value, 10, 40, w - 20, 28, accent, 15F, true);
        }

        private Label AddLabel(Control parent, string text, int x, int y, int w, int h, Color color, float size, bool bold)
        {
            var label = new Label
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(w, h),
                ForeColor = color,
                Font = new Font("Microsoft JhengHei UI", size, bold ? FontStyle.Bold : FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft
            };
            parent.Controls.Add(label);
            return label;
        }

        private TextBox CreateTextBox(Control parent, string label, int x, int y, int w)
        {
            AddLabel(parent, label, x, y - 24, w, 22, TextSub, 9F, true);
            var box = new TextBox { Location = new Point(x, y), Width = w, Height = 30, Font = new Font("Microsoft JhengHei UI", 10F), BorderStyle = BorderStyle.FixedSingle, BackColor = InputBg, ForeColor = TextMain };
            parent.Controls.Add(box);
            return box;
        }

        private ComboBox CreateCombo(Control parent, string label, int x, int y, int w, string[] items)
        {
            AddLabel(parent, label, x, y - 24, w, 22, TextSub, 9F, true);
            var combo = new ComboBox { Location = new Point(x, y), Width = w, Height = 30, Font = new Font("Microsoft JhengHei UI", 10F), DropDownStyle = ComboBoxStyle.DropDownList, BackColor = InputBg, ForeColor = TextMain };
            combo.Items.AddRange(items);
            if (combo.Items.Count > 0) combo.SelectedIndex = 0;
            parent.Controls.Add(combo);
            return combo;
        }

        private ComboBox CreateMemberCombo(Control parent, string label, int x, int y, int w)
        {
            AddLabel(parent, label, x, y - 24, w, 22, TextSub, 9F, true);
            var combo = new ComboBox { Location = new Point(x, y), Width = w, Height = 30, Font = new Font("Microsoft JhengHei UI", 10F), DropDownStyle = ComboBoxStyle.DropDownList, BackColor = InputBg, ForeColor = TextMain };
            BindMemberCombo(combo, GetSelectedHeaderMemberId());
            parent.Controls.Add(combo);
            return combo;
        }

        private DateTimePicker CreateDatePicker(Control parent, string label, int x, int y, int w)
        {
            AddLabel(parent, label, x, y - 24, w, 22, TextSub, 9F, true);
            var picker = new DateTimePicker { Location = new Point(x, y), Width = w, Height = 30, Format = DateTimePickerFormat.Short, Font = new Font("Microsoft JhengHei UI", 10F), CalendarTitleBackColor = NavBg, CalendarTitleForeColor = NavActive };
            parent.Controls.Add(picker);
            return picker;
        }

        private NumericUpDown CreateNumber(Control parent, string label, int x, int y, int w, decimal min, decimal max, int decimalPlaces)
        {
            AddLabel(parent, label, x, y - 24, w, 22, TextSub, 9F, true);
            var num = new NumericUpDown
            {
                Location = new Point(x, y),
                Width = w,
                Height = 30,
                Minimum = min,
                Maximum = max,
                DecimalPlaces = decimalPlaces,
                Increment = decimalPlaces == 0 ? 1 : 0.1M,
                Font = new Font("Microsoft JhengHei UI", 10F),
                BackColor = InputBg,
                ForeColor = TextMain
            };
            if (label.Contains("身高")) num.Value = 170;
            else if (label.Contains("體重")) num.Value = 70;
            else if (label.Contains("體脂")) num.Value = 22;
            else if (label.Contains("肌肉")) num.Value = 45;
            else if (label.Contains("內臟")) num.Value = 7;
            else if (label.Contains("腰圍")) num.Value = 80;
            else if (label.Contains("臀圍")) num.Value = 92;
            parent.Controls.Add(num);
            return num;
        }

        private Button CreateButton(string text, Color color, int width)
        {
            var btn = new Button
            {
                Text = text,
                Width = width,
                Height = 38,
                BackColor = color,
                ForeColor = color.GetBrightness() > 0.55F ? Color.Black : Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Microsoft JhengHei UI", 9.5F, FontStyle.Bold)
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private DataGridView CreateGrid(Control parent, int x, int y, int w, int h)
        {
            var grid = new DataGridView
            {
                Location = new Point(x, y),
                Size = new Size(w, h),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false,
                BackgroundColor = CardBg,
                BorderStyle = BorderStyle.FixedSingle,
                GridColor = Border,
                Font = new Font("Microsoft JhengHei UI", 9.5F),
                ColumnHeadersHeight = 36,
                RowTemplate = { Height = 32 }
            };
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 0, 0);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = NavActive;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft JhengHei UI", 9.5F, FontStyle.Bold);
            grid.EnableHeadersVisualStyles = false;
            grid.DefaultCellStyle.BackColor = InputBg;
            grid.DefaultCellStyle.ForeColor = TextMain;
            grid.DefaultCellStyle.SelectionBackColor = NavActive;
            grid.DefaultCellStyle.SelectionForeColor = Color.Black;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(31, 31, 31);
            parent.Controls.Add(grid);
            return grid;
        }

        private Chart CreateProgressChart(int memberId)
        {
            var chart = new Chart { BackColor = CardBg };
            var area = new ChartArea("Main");
            area.BackColor = CardBg;
            area.AxisX.MajorGrid.LineColor = Border;
            area.AxisY.MajorGrid.LineColor = Border;
            area.AxisX.LabelStyle.ForeColor = TextSub;
            area.AxisY.LabelStyle.ForeColor = TextSub;
            area.AxisX.LineColor = Border;
            area.AxisY.LineColor = Border;
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 8F);
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8F);
            area.AxisX.Interval = 1;
            chart.ChartAreas.Add(area);

            AddLineSeries(chart, "體重 kg", Blue);
            AddLineSeries(chart, "體脂 %", Orange);
            AddLineSeries(chart, "肌肉 kg", Green);

            var data = bodyRecords.Where(r => r.MemberId == memberId).OrderBy(r => r.RecordDate).ToList();
            foreach (var r in data)
            {
                string x = r.RecordDate.ToString("MM/dd");
                chart.Series["體重 kg"].Points.AddXY(x, r.WeightKg);
                chart.Series["體脂 %"].Points.AddXY(x, r.BodyFatPct);
                chart.Series["肌肉 kg"].Points.AddXY(x, r.MuscleKg);
            }

            var legend = new Legend("Legend") { Docking = Docking.Top, Alignment = StringAlignment.Center, Font = new Font("Microsoft JhengHei UI", 9F), BackColor = CardBg, ForeColor = TextMain };
            chart.Legends.Add(legend);
            return chart;
        }

        private void AddLineSeries(Chart chart, string name, Color color)
        {
            var s = new Series(name)
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Color = color,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 7,
                XValueType = ChartValueType.String
            };
            chart.Series.Add(s);
        }

        private void RefreshHeaderMemberCombo()
        {
            if (cboHeaderMember == null) return;
            int oldId = GetSelectedHeaderMemberId();
            isBinding = true;
            cboHeaderMember.DataSource = null;
            cboHeaderMember.Items.Clear();
            foreach (var m in members.OrderBy(x => x.Id)) cboHeaderMember.Items.Add(m);
            if (cboHeaderMember.Items.Count > 0)
            {
                int index = 0;
                for (int i = 0; i < cboHeaderMember.Items.Count; i++)
                {
                    var m = cboHeaderMember.Items[i] as Member;
                    if (m != null && m.Id == oldId) { index = i; break; }
                }
                cboHeaderMember.SelectedIndex = index;
            }
            isBinding = false;
        }

        private void BindMemberCombo(ComboBox combo, int selectedId)
        {
            combo.Items.Clear();
            foreach (var m in members.OrderBy(x => x.Id)) combo.Items.Add(m);
            if (combo.Items.Count > 0)
            {
                int index = 0;
                for (int i = 0; i < combo.Items.Count; i++)
                {
                    var m = combo.Items[i] as Member;
                    if (m != null && m.Id == selectedId) { index = i; break; }
                }
                combo.SelectedIndex = index;
            }
        }

        private int GetSelectedHeaderMemberId()
        {
            if (cboHeaderMember != null && cboHeaderMember.SelectedItem is Member)
                return ((Member)cboHeaderMember.SelectedItem).Id;
            return members.Count > 0 ? members[0].Id : -1;
        }

        private int GetComboMemberId(ComboBox combo)
        {
            if (combo != null && combo.SelectedItem is Member) return ((Member)combo.SelectedItem).Id;
            return GetSelectedHeaderMemberId();
        }

        private Member GetMember(int id)
        {
            return members.FirstOrDefault(m => m.Id == id);
        }

        private string GetMemberName(int id)
        {
            var m = GetMember(id);
            return m == null ? "未知會員" : m.Name;
        }

        private BodyRecord GetLatestRecord(int memberId)
        {
            return bodyRecords.Where(r => r.MemberId == memberId).OrderByDescending(r => r.RecordDate).FirstOrDefault();
        }

        private double CalculateBmi(Member m, BodyRecord r)
        {
            if (m == null || r == null || m.HeightCm <= 0) return 0;
            double meter = m.HeightCm / 100.0;
            return r.WeightKg / (meter * meter);
        }

        private string DeltaText(double delta, string unit)
        {
            if (Math.Abs(delta) < 0.05) return "與第一筆幾乎相同";
            return (delta > 0 ? "+" : "") + delta.ToString("0.0") + " " + unit + "，相較第一筆";
        }

        private double ParseActivityFactor(string text)
        {
            if (text.Contains("1.725")) return 1.725;
            if (text.Contains("1.55")) return 1.55;
            if (text.Contains("1.375")) return 1.375;
            return 1.2;
        }

        private DateTime SafeDate(DateTime d)
        {
            if (d < DateTimePicker.MinimumDateTime) return DateTime.Today;
            if (d > DateTimePicker.MaximumDateTime) return DateTime.Today;
            return d;
        }

        private decimal ClampDecimal(decimal value, decimal min, decimal max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}

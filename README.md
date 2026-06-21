# FitPro Manager

**FitPro Manager** 是一套使用 **C# Windows Forms App (.NET Framework 4.7.2)** 製作的健身房會員管理系統，主要用於管理健身房會員資料、身體組成紀錄、成長軌跡、飲食建議、會籍付款與報表匯出。

本專案適合作為「視窗程式設計」期末專題，系統具備會員 CRUD、CSV 讀寫檔、會員照片上傳、資料視覺化圖表、飲食規劃與 TXT 報表匯出功能。整體介面採用黑色與黃色主題，讓系統更接近實際健身房後台管理系統。

---

## 一、專案特色

- 主畫面 Dashboard：顯示會員總數、有效會籍、即將到期會員與身體紀錄數量。
  <img width="1409" height="793" alt="image" src="https://github.com/user-attachments/assets/b877a391-da56-414b-980b-20a8f86f24d2" />

- 會員資料管理：新增、修改、刪除、搜尋會員。
- 會員照片：可選擇 JPG / PNG / BMP 作為會員個人照片，儲存時會複製到 `Data/Photos`。
  <img width="1416" height="795" alt="image" src="https://github.com/user-attachments/assets/56d9d491-b345-44c6-8369-69b58bc5c311" />

- 身體分析：記錄體重、體脂率、肌肉量、內臟脂肪、腰圍、臀圍。
  <img width="1414" height="798" alt="image" src="https://github.com/user-attachments/assets/5d489bdb-35fa-4ce9-bc8d-8d68c4c9fb8c" />

- 成長軌跡：使用折線圖顯示體重、體脂率、肌肉量變化。
  <img width="1422" height="790" alt="image" src="https://github.com/user-attachments/assets/3385b020-41f9-43f5-bdf5-03a7e0690d20" />

- 飲食規劃：依照會員資料與最新身體紀錄計算 BMR、TDEE、建議熱量與三大營養素。
  <img width="1419" height="795" alt="image" src="https://github.com/user-attachments/assets/db7ff685-bee4-4249-b9e8-671dff883879" />

- 付款與報表：新增會籍付款、自動更新到期日、匯出 TXT 報表。
  <img width="1409" height="805" alt="image" src="https://github.com/user-attachments/assets/9fa642c1-20d7-48b3-bd1c-10e5f22bd8aa" />

- 讀寫檔功能：會員、身體紀錄、付款紀錄皆使用 CSV 儲存。
- UI 設計：黑黃配色、左側導覽列、分類卡片，操作流程清楚。

---

## 二、開發環境

| 項目 | 內容 |
|---|---|
| 開發語言 | C# |
| 專案類型 | Windows Forms App (.NET Framework) |
| Target Framework | .NET Framework 4.7.2 |
| 開發工具 | Visual Studio |
| UI 技術 | Windows Forms |
| 圖表控制項 | System.Windows.Forms.DataVisualization.Charting |
| 資料儲存 | CSV / TXT 檔案 |
| 執行平台 | Windows |

---

## 三、執行方式

1. 解壓縮專案資料夾。
2. 使用 Visual Studio 開啟 `GymMemberSystem.sln`。
3. 確認專案類型為 `Windows Forms App (.NET Framework)`。
4. 按下 `F5` 執行。
5. 程式開啟後會進入 FitPro Manager 主畫面。

---

## 四、主要功能說明

### 1. 主畫面 Dashboard

主畫面會先將系統功能分類，不會一開始就把所有資料擠在同一個畫面。畫面上會顯示：

- 會員總數
- 有效會籍
- 即將到期會員
- 身體紀錄總數

下方提供五大功能入口：

1. 會員資料
2. 身體分析
3. 成長軌跡
4. 飲食規劃
5. 付款與報表

### 2. 會員資料管理

會員資料頁面可以新增、修改、刪除與搜尋會員。會員資料包含：

- 姓名
- 電話
- 性別
- 生日
- 身高
- 訓練目標
- 負責教練
- 入會日期
- 到期日期
- 會員照片

系統會根據會員的到期日自動判斷會員狀態，例如「有效」或「過期」。

### 3. 會員照片功能

在會員資料頁面可以按下「選擇照片」，選擇 JPG、PNG 或 BMP 圖片。儲存會員後，系統會自動將照片複製到：

```text
Data/Photos/
```

之後點選會員時，系統會自動載入對應照片，讓會員資料更完整。

### 4. 身體分析

身體分析頁面可記錄會員每次量測的身體數據，概念類似健身房常見的 TANITA / InBody 身體組成紀錄。

可記錄資料包含：

- 體重 kg
- 體脂率 %
- 肌肉量 kg
- 內臟脂肪
- 腰圍 cm
- 臀圍 cm

### 5. 成長軌跡

成長軌跡頁面會讀取會員歷史身體紀錄，並用折線圖呈現：

- 體重變化
- 體脂率變化
- 肌肉量變化

這可以讓教練或會員快速查看訓練成果。

### 6. 飲食規劃

飲食規劃頁面會根據會員的性別、年齡、身高、最新體重、活動量與目標計算：

- BMR 基礎代謝率
- TDEE 每日總消耗熱量
- 建議每日熱量
- 蛋白質攝取量
- 脂肪攝取量
- 碳水化合物攝取量
- 一日菜單範例

系統也可以將飲食規劃匯出成 TXT 檔案。

### 7. 付款與報表

付款與報表頁面可新增會員會籍付款紀錄，並依照方案自動更新會員到期日。

支援方案：

- 單次入場
- 月卡
- 季卡
- 半年卡
- 年卡

支援付款方式：

- 現金
- 信用卡
- 轉帳
- Line Pay

此頁也可匯出會員摘要與身體分析 TXT 報表。

---

## 五、資料儲存位置

程式執行後會自動建立：

```text
bin/Debug/Data/
├─ members.csv
├─ body_records.csv
├─ payments.csv
└─ Photos/
```

檔案用途：

- `members.csv`：儲存會員基本資料。
- `body_records.csv`：儲存會員身體分析紀錄。
- `payments.csv`：儲存會員付款紀錄。
- `Photos/`：儲存會員照片。

---

## 六、操作流程建議

錄製五分鐘 展示 時，建議照以下順序：

1. 介紹專題名稱與開發工具。
2. 展示主畫面 Dashboard 與五大分類。
3. 到「會員資料」新增一位會員並選擇照片。
4. 到「身體分析」新增一筆量測紀錄。
5. 到「成長軌跡」查看折線圖。
6. 到「飲食規劃」產生 BMR、TDEE 與菜單。
7. 到「付款與報表」新增付款並匯出報表。
8. 最後說明系統有 CSV / TXT 讀寫檔功能。

---

## 七、專案結構

```text
FitProManager_MainScreen/
├─ GymMemberSystem.sln
├─ GymMemberSystem.csproj
├─ Program.cs
├─ MainForm.cs
├─ Models.cs
├─ DataStore.cs
├─ README.md
├─ 使用說明書草稿.md
└─ .gitignore
```

### Program.cs

程式進入點，負責啟動 `MainForm`。

### MainForm.cs

主要視窗與 UI 邏輯，包含主畫面、會員資料、身體分析、成長軌跡、飲食規劃、付款與報表。

### Models.cs

定義會員、身體紀錄、付款紀錄等資料模型。

### DataStore.cs

負責 CSV / TXT 讀寫、會員照片複製、報表匯出。

---

## 八、GitHub 注意事項

上傳 GitHub 時請不要上傳 Visual Studio 產生的編譯檔與暫存檔。本專案已提供 `.gitignore`，會排除：

```text
.vs/
bin/
obj/
*.user
*.suo
*.cache
Data/
```

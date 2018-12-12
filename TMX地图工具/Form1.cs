using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Windows.Forms;
using TiledSharp;

namespace TMX地图工具 {
    public partial class Form1: Form {
        public TmxMap 世界地图_TMX = null;

        public int 地图宽度 = 300;
        public int 地图高度 = 300;

        public int[,] 遮罩;
        int 大地图标记起始索引 = -1;
        int 随机地图标记起始索引 = -1;

        public int[,] 地型地图;
        public int[,] 土地等级地图;
        public int[,] 资源等级地图;

        public int[,] Layer2映射地图;

        public int 地型_泥地 = 4;
        public int 地型_草地 = 7;
        public int 地型_湖泊 = 10;

        public int 树林 = 6;
        public int 山体 = 3;

        public int 湖泊装饰 = 12;
        public int 陆地装饰 = 14;

        int black = 0;
        int white = 1;
        int red = 2;
        int brown = 4;
        int yellow = 5;
        int green = 7;
        int blue = 10;
        int purple = 14;


        List<Size> 相邻四边偏移量 = new List<Size> { new Size(-1, 0), new Size(0, -1), new Size(1, 0), new Size(0, 1) };

        List<Size> 相邻八边偏移量 = new List<Size> {
        new Size(-1, -1),
        new Size(0, -1),
        new Size(1, -1),
        new Size(1, 0),
        new Size(1, 1),
        new Size(0, 1),
        new Size(-1, 1),
        new Size(-1, 0),
    };
        public Form1() {
            InitializeComponent();
        }

        private void btn_brow_Click( object sender, EventArgs e ) {

            var path = GetFilePathDialog("Tiled地图(*.tmx)|*.tmx|所有文件|*.*", "WorldMap_New", "tmx");
            if(path != null ) {

                tb_path.Text = path;
                世界地图_TMX = null;
            }
        }

        private void btn_export_excel_Click( object sender, EventArgs e ) {

            if( !TmxIsLoaded() ) {
                return;
            }

            string savePath = OpenSaveFileDialog("Excel表格(*.xlsx)|*.xlsx|所有文件|*.*", "WorldMap_New", "xlsx", true);
            if( savePath == null ) {
                return;
            }

            IWorkbook workbook = new XSSFWorkbook();

            ISheet sheet1 = workbook.CreateSheet("Sheet1");

            //sheet1.CreateRow(0).CreateCell(0).SetCellValue("This is a Sample");


            IRow row = sheet1.CreateRow(0);
            row.CreateCell(0).SetCellValue("坐标");
            for ( int i = 0; i < 世界地图_TMX.Layers.Count; i++ ) {
                row.CreateCell(i + 1).SetCellValue(世界地图_TMX.Layers[i].Name);
            }

            int count = 1;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 地图高度 * 地图宽度;


            for ( int y = 0; y < 地图高度; y++ ) {
                for ( int x = 0; x < 地图宽度; x++ ) {
                    row = sheet1.CreateRow(count);                

                    row.CreateCell(0).SetCellValue(string.Format("{0}-{1}", x, y));

                    for ( int k = 0; k < 世界地图_TMX.Layers.Count; k++ ) {
                        var value = 世界地图_TMX.Layers[k].Tiles[x + y * 地图宽度].Gid;
                        row.CreateCell(k + 1).SetCellValue(value);
                    }

                    progressBar1.Value = count;
                    count++;
                }
            }

            FileStream sw = File.Create(savePath);

            workbook.Write(sw);

            sw.Close();
            MessageBox.Show("导出完成!");

        }
        #region 地图整体随机模块

        private void 地图整体随机( object sender, EventArgs e ) {
            int 总模块 = 5;
            int 模块占比 = 10;

            progressBar1.Minimum = 0;
            progressBar1.Maximum = 总模块 * 模块占比;
            progressBar1.Value = 0;

            var path = OpenSaveFileDialog("Tiled地图(*.tmx)|*.tmx|所有文件|*.*", "WorldMap_New", "tmx");
            if ( path == null ) {
                MessageBox.Show("取消地图随机!");
                return;
            }

            //模块1
            if ( !TmxIsLoaded() ) {
                return ;
            }
            progressBar1.Value += 模块占比;

            //模块2
            if ( !地图数组初始化() ) {
                return;
            }
            progressBar1.Value += 模块占比;

            //模块3
            地型随机();
            progressBar1.Value += 模块占比;

            //模块4
            装饰物随机();
            progressBar1.Value += 模块占比;

            //模块5
            if ( 地图数据转化成CSV格式() ) {
                SaveTmx(path);
            }
            else {
                MessageBox.Show("地图数据转化成CSV格式失败!");
            }
            progressBar1.Value += 模块占比;
        }

        private bool 地图数组初始化() {
            地型地图 = new int[地图宽度, 地图高度];
            Layer2映射地图 = new int[地图宽度, 地图高度];


            遮罩 = new int[地图宽度, 地图高度];
            if ( !SetGidOfLayer("Mask", ref 遮罩) ) {//收集每个郡区域
                MessageBox.Show("遮罩加载错误!");
                return false;
            }

            return true;
        }

        void 地图重置(int[,] 地图) {
            地图 = new int[地图宽度, 地图高度];
        }

        public bool 地图数据转化成CSV格式() {

            int[,] Terrain = new int[地图宽度, 地图高度];
            int[,] Landform = new int[地图宽度, 地图高度];

            for ( int col, row = 0; row < 地图高度; row++ ) {
                for ( col = 0; col < 地图宽度; col++ ) {

                    Terrain[col, row] = 地型地图[col, row] + 随机地图标记起始索引;
                    Landform[col, row] = Layer2映射地图[col, row] + 随机地图标记起始索引;
                }
            }

            List<int[,]> 待转化数组列表 = new List<int[,]> { Terrain, Landform };
            List<string> 图层名字列表 = new List<string>() { "Terrain", "Landform" };

            return  SaveDataForEachLayer(待转化数组列表, 图层名字列表);
        }

        #region 地型随机

        float 湖泊概率 = 0.1f;
        float 草地概率 = 0.4f;

        int 已生成湖泊格子数;
        int 已生成草地格子数;
        void 地型随机() {
            地图重置(地型地图);
            
            已生成湖泊格子数 = 0;

            List<Point> 待生成湖泊集合 = new List<Point>();
            List<Point> 待生成草地集合 = new List<Point>();

            for ( int row = 0; row < 地图高度; row++ ) {
                for ( int col = 0; col < 地图宽度; col++ ) {

                    Point pos = new Point(col, row);


                    if ( IsLakeInMask(pos) ) {

                        设置湖泊(pos);
                    }
                    else {


                        地型地图[col, row] = 地型_泥地;

                        //if ( Range(1, 100) < 沙地概率 ) {
                        //}
                        //else {

                        //    地型地图[col, row] = 地型_草地;
                        //}

                        if ( 当前坐标N格范围内没有特定地型(pos, 2, IsLake) ) {

                            待生成草地集合.Add(pos);
                        }

                        if ( 可生成河流(pos) ) {

                            待生成湖泊集合.Add(pos);
                        }
                    }
                }
            }

            //湖泊
            while ( 待生成湖泊集合.Count != 0 && !已生成湖泊到达总占比() ) {

                var pos = 待生成湖泊集合.Random();

                地型格子数 = Range(9, 20);

                if ( 当前坐标N格范围内没有特定地型(pos, 20, IsLake) ) {

                    地型随机延伸(pos, 可生成河流, 设置湖泊);
                }

                待生成湖泊集合.Remove(pos);
            }

            //草地
            已生成草地格子数 = 0;

            while ( 待生成草地集合.Count != 0 && !已生成草地到达总占比() ) {

                var pos = 待生成草地集合.Random();

                地型格子数 = Range(5, 200);

                if ( 可生成草地(pos) ) {

                    地型随机延伸(pos, 可生成草地, 设置草地);
                }

                待生成草地集合.Remove(pos);
            }

        }

        void 设置草地( Point pos ) {

            地型地图[pos.X, pos.Y] = 地型_草地;
            已生成草地格子数++;
        }

        bool 已生成草地到达总占比() {

            return 已生成草地格子数 > ( TotalPos() * 草地概率 );
        }

        bool 可生成草地( Point pos ) {
            if ( IsGrass(pos) ||
                已生成草地到达总占比() ||
                当前坐标N格范围包含特定地型(pos, 2, IsLake)
                ) {

                return false;
            }

            return true;
        }

        void 设置湖泊( Point pos ) {

            地型地图[pos.X, pos.Y] = 地型_湖泊;
            已生成湖泊格子数++;           
        }

        bool 已生成湖泊到达总占比() {

            return 已生成湖泊格子数 > ( TotalPos() * 湖泊概率 );
        }

        bool 可生成河流( Point pos ) {

            if( IsLake(pos) ||
                IsBlackInMask(pos) ||
                已生成湖泊到达总占比 () ||
                当前坐标N格范围包含特定地型(pos, 2, IsMountInMask) ||
                当前坐标N格范围包含特定地型(pos, 2, IsForestInMask) ||
                当前坐标N格范围包含特定地型(pos, 2, IsWhiteMask)
                ) {

                return false;
            }
            return true;
        }

        #endregion

        #region  装饰物随机

        float 山体概率 = 0.20f * ( 0.9f );
        float 树林概率 = 0.35f * ( 0.9f );

        float 湖泊装饰物概率 = 0.05f;
        float 陆地装饰物概率 = 0.05f;

        int 已生成的山体格子;
        int 已生成的树林格子;
        int 已生成的湖泊装饰格子;
        int 已生成的陆地装饰格子;
        void 装饰物随机() {
            地图重置(Layer2映射地图);

            已生成的山体格子 = 0;
            已生成的树林格子 = 0;
            已生成的湖泊装饰格子 = 0;
            已生成的陆地装饰格子 = 0;

            List<Point> 全图点集合 = new List<Point>();
            List<Point> 遮罩空白区域点集合 = new List<Point>();

            for ( int row = 0; row < 地图高度; row++ ) {
                for ( int col = 0; col < 地图宽度; col++ ) {

                    var pos = new Point(col, row);
                    全图点集合.Add(pos);

                    if ( IsMountInMask(pos) ) {

                        设置layer2层山体(pos);
                    }


                    if ( IsForestInMask(pos) ) {

                        设置layer2层树林(pos);
                    }

                    if ( IsBlankInMask(pos) ||
                        IsBlackInMask(pos)
                        ) {

                        遮罩空白区域点集合.Add(pos);
                    }
                }
            }

            while ( 遮罩空白区域点集合.Count != 0 && !山体达到总占比() ) {

                var pos = 遮罩空白区域点集合.Random();

                if ( 该位置可放山体(pos) ) {

                    地型格子数 = Range(9, 20);

                    地型随机延伸(pos, 该位置可放山体, 设置layer2层山体);
                }

                遮罩空白区域点集合.Remove(pos);
            }

            while ( 遮罩空白区域点集合.Count != 0 && !树林达到总占比() ) {

                var pos = 遮罩空白区域点集合.Random();

                if( 该位置可放树林(pos) ) {

                    地型格子数 = Range(9, 20);

                    地型随机延伸(pos, 该位置可放树林, 设置layer2层树林);
                }

                遮罩空白区域点集合.Remove(pos);
            }

            while ( 全图点集合.Count != 0 && (!湖泊装饰物达到总占比() || !陆地装饰物达到总占比()) ) {
                var pos = 全图点集合.Random();

                if ( 该位置可放湖泊装饰物(pos) ) {
                    设置湖泊装饰物(pos);
                }

                if( 该位置可放陆地装饰物(pos) )
                {
                    设置陆地装饰物(pos);
                }

                全图点集合.Remove(pos);
            }
        }

        void 设置湖泊装饰物( Point pos ) {

            Layer2映射地图[pos.X, pos.Y] = 湖泊装饰;
            已生成的湖泊装饰格子++;
        }

        void 设置陆地装饰物( Point pos ) {

            Layer2映射地图[pos.X, pos.Y] = 陆地装饰;
            已生成的陆地装饰格子++;
        }

        bool 该位置可放湖泊装饰物( Point pos ) {
            if ( HasNotArticle(pos) &&
                !湖泊装饰物达到总占比() &&
                IsLake(pos)
                ) {
                return true;
            }
            return false;
        }

        bool 该位置可放陆地装饰物( Point pos ) {
            if ( HasNotArticle(pos) &&
                !陆地装饰物达到总占比() &&
                !IsWhiteMask(pos) &&
                当前坐标N格范围内没有特定地型(pos, 1, IsLake)
                ) {
                return true;
            }
            return false;
        }

        bool 湖泊装饰物达到总占比() {

            return 已生成的湖泊装饰格子 > ( TotalPos() * 湖泊装饰物概率 );
        }

        bool 陆地装饰物达到总占比() {

            return 已生成的陆地装饰格子 > ( TotalPos() * 陆地装饰物概率 );
        }

        bool 树林达到总占比() {

            return 已生成的树林格子 > ( TotalPos() * 树林概率 );
        }

        bool 该位置可放树林( Point pos ) {
            if ( ( !IsBlankInMask(pos) && !IsBlackInMask(pos) ) ||
                树林达到总占比() ||
                IsForest(pos) ||
                IsMount(pos) ||
                当前坐标N格范围包含特定地型(pos, 1, IsLake)
                ) {
                return false;
            }
            return true;
        }

        bool 山体达到总占比() {

            return 已生成的山体格子 > ( TotalPos() * 山体概率 );
        }

        bool 该位置可放山体( Point pos ) {
            if ( ( !IsBlankInMask(pos) && !IsBlackInMask(pos) ) ||
                山体达到总占比() ||
                IsForest(pos) ||
                IsMount(pos) ||
                当前坐标N格范围包含特定地型(pos, 1, IsLake)
                ) {
                return false;
            }
            return true;
        }

        void 设置layer2层树林( Point pos ) {

            Layer2映射地图[pos.X, pos.Y] = 树林;
            已生成的树林格子++;
        }

        void 设置layer2层山体( Point pos ) {

            Layer2映射地图[pos.X, pos.Y] = 山体;
            已生成的山体格子++;
        } 

        #endregion

        #region 地图土地等级生成
        int 内圈内边界;
        int 内圈外边界;
        int 外圈内边界;
        int 外圈外边界;
        Point 地图中心;

        int 地块等级起始 = 15;
        private void 地图土地等级生成() {

            //边界距离地图中心的宽度
            内圈内边界 = (int)( 地图宽度 * 9.0 / 100 );
            内圈外边界 = (int)( 地图宽度 * 15.0 / 100 );
            外圈内边界 = (int)( 地图宽度 * 27.0 / 100 );
            外圈外边界 = (int)( 地图宽度 * 35.0 / 100 );

            地图中心 = new Point(( 地图宽度 - 1 ) / 2, ( 地图高度 - 1 ) / 2);

            for ( int row = 0; row < 地图高度; row++ ) {
                for ( int col = 0; col < 地图宽度; col++ ) {
                    Point pos = new Point(col, row);

                    土地等级地图[col, row] = 返回地块随机等级(地块区域(pos)) + 地块等级起始;
                }
            }
        }


        int[,] 每个区域每个等级概率表 = new int[5, 9] {
             { 30,40,50, 0, 0, 0, 0, 0, 0},
             {  0, 5,25,50,20, 0, 0, 0, 0},
             {  0, 0, 0,15,35,50, 0, 0, 0},
             {  0, 0, 0, 0,10,15,45,30, 0},
             {  0, 0, 0, 0, 0, 0,20,30,50},
         };
        public int 返回地块随机等级( int 区域号 ) {

            if ( 区域号 < 0 || 区域号 > 每个区域每个等级概率表.Length - 1 ) {
                Console.WriteLine("该区域不存在!");
                return -1;
            }
            int random = Range(1, 100);
            int sum = 0;

            for ( int i = 0; i < 9; i++ ) {
                sum += 每个区域每个等级概率表[区域号, i];
                if ( random <= sum ) {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 区域由里到外:4-0
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public int 地块区域( Point pos ) {

            int 距离 = Distance(地图中心, pos);

            if ( 距离 < 内圈内边界 ) {
                return 4;
            }
            else if ( 距离 < 内圈外边界 ) {
                return 3;
            }
            else if ( 距离 < 外圈内边界 ) {
                return 2;
            }
            else if ( 距离 < 外圈外边界 ) {
                return 1;
            }
            else if ( 距离 <= 地图宽度 / 2 ) {
                return 0;
            }

            return -1;
        }


        public int 地块等级( int x, int y ) {
            return 土地等级地图[x, y] - 地块等级起始;
        }
        #endregion

        #region 资源等级生成

        //地块等级对应的生成概率
        int[,] 资源等级概率生成表 = new int[,] {
        {50,00,00,00,00,50,00,00,00,00,00,00,00,00,00,00,00,00,00,00,},
        {20,30,00,00,00,20,30,00,00,00,00,00,00,00,00,00,00,00,00,00,},
        {00,33,00,00,00,00,33,00,00,00,34,00,00,00,00,00,00,00,00,00,},
        {00,00,33,00,00,00,00,33,00,00,00,34,00,00,00,00,00,00,00,00,},
        {00,00,10,20,00,00,00,10,20,00,00,10,20,00,00,10,00,00,00,00,},
        {00,00,00,25,00,00,00,00,25,00,00,00,25,00,00,00,25,00,00,00,},
        {00,00,00,00,25,00,00,00,00,25,00,00,00,25,00,00,00,25,00,00,},
        {00,00,00,00,20,00,00,00,00,20,00,00,00,15,10,00,00,00,30,05,},
        {00,00,00,00,25,00,00,00,00,25,00,00,00,00,25,00,00,00,00,25,},
    };
        string[] 资源等级表 = {
        "10150000农田Lv1" ,
        "10150000农田Lv2" ,
        "10150000农田Lv3" ,
        "10150000农田Lv4" ,
        "10150000农田Lv5" ,
        "10160000石矿Lv1" ,
        "10160000石矿Lv2" ,
        "10160000石矿Lv3" ,
        "10160000石矿Lv4" ,
        "10160000石矿Lv5" ,
        "10170000铁矿Lv1" ,
        "10170000铁矿Lv2" ,
        "10170000铁矿Lv3" ,
        "10170000铁矿Lv4" ,
        "10170000铁矿Lv5" ,
        "10180000铜矿Lv1" ,
        "10180000铜矿Lv2" ,
        "10180000铜矿Lv3" ,
        "10180000铜矿Lv4" ,
        "10180000铜矿Lv5" ,
    };

        int 资源初始等级 = 43;
        private void 资源等级生成() {

            for ( int col, row = 0; row < 地图高度; row++ ) {
                for ( col = 0; col < 地图宽度; col++ ) {

                    int 等级 = 地块等级(col, row);
                    int random = Range(1, 100);
                    int sum = 0;

                    for ( int i = 0; i < 20; i++ ) {
                        sum += 资源等级概率生成表[等级, i];
                        if ( random <= sum ) {
                            资源等级地图[col, row] = 资源初始等级 + i;
                            break;
                        }
                    }

                }
            }
        }

        public string 资源等级( int x, int y ) {
            return 资源等级表[资源等级地图[x, y] - 资源初始等级];
        }

        #endregion

        #endregion

        #region 公用函数

        public void Log(string msg) {
            Console.WriteLine(msg);
        }

        public static int Distance( Point pos, Point pos2 ) {
            return Math.Max(Math.Abs(pos.X - pos2.X), Math.Abs(pos.Y - pos2.Y));
        }
        /// <summary>
        /// 返回min - max之间的整数,包含min和max.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Range( int min, int max ) {
            var seed = Guid.NewGuid().GetHashCode();
            Random ran = new Random(seed);
            
            return ran.Next(min, max + 1);
        }


        public bool OneIn( int n ) {
            if ( Range(1, n) == 1 ) {
                return true;
            }
            return false;
        }

        public static void ProcessCommand( string command, string argument ) {
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(command);
            info.Arguments = argument;
            info.CreateNoWindow = false;
            info.ErrorDialog = true;
            info.UseShellExecute = false;

            if ( info.UseShellExecute ) {
                info.RedirectStandardOutput = false;
                info.RedirectStandardError = false;
                info.RedirectStandardInput = false;
            }
            else {
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;
                info.RedirectStandardInput = true;
            }

            System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);

            if ( !info.UseShellExecute ) {
                //Debug.Log(process.StandardOutput);
                //Debug.Log(process.StandardError);
            }

            //process.WaitForExit();
            process.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Filter">设置文件类型</param>
        /// <param name="FileName">设置默认文件名</param>
        /// <param name="DefaultExt">设置默认格式（可以不设）</param>
        /// <param name="judgeSharing"></param>
        /// <returns></returns>
        public string OpenSaveFileDialog( string Filter, string FileName, string DefaultExt, bool judgeSharing = false ) {

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = Filter;//设置文件类型
            sfd.FileName = FileName;//设置默认文件名
            sfd.DefaultExt = DefaultExt;//设置默认格式（可以不设）
            sfd.AddExtension = true;//设置自动在文件名中添加扩展名

            if ( sfd.ShowDialog() == DialogResult.OK ) {
                if ( judgeSharing && IsFileOpen(sfd.FileName) ) {
                    MessageBox.Show("文件已打开!请先关闭再保存或保存为其他文件名.");
                    return null;
                }
                return sfd.FileName;
            }
            else {
                return null;
            }

        }

        public string GetFilePathDialog( string Filter, string FileName, string DefaultExt ) {

            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Filter = Filter;
            OFD.FileName = FileName;
            OFD.DefaultExt = DefaultExt;
            OFD.RestoreDirectory = true;
            OFD.FilterIndex = 1;

            if ( OFD.ShowDialog() == DialogResult.OK ) {
                return OFD.FileName;
            }
            //else {
            //    MessageBox.Show("取消选择!");
            //}
            return null;
        }

        public void SaveTmx(string filePath) {
            if ( filePath != null ) {
                //保存TMX地图
                StreamWriter swriter = new StreamWriter(filePath, false, new UTF8Encoding(false));
                世界地图_TMX.xDoc.Save(swriter);
                swriter.WriteLine();
                swriter.Close();

                string sPath = Environment.GetEnvironmentVariable("Tiled");
                //Debug.Log(sPath);
                ProcessCommand(sPath, filePath);
            }
        }

        bool SaveDataForEachLayer( List<int[,]> 待转化数组列表, List<string> 图层名字列表 ) {

            var encoding = 世界地图_TMX.Layers[0].encoding;
            if ( encoding == "csv" ) {

                List<string> csv数组列表 = new List<string>();
                List<string[]> 每行数据列表 = new List<string[]>();

                for ( int i = 0; i < 图层名字列表.Count; i++ ) {
                    csv数组列表.Add(Environment.NewLine);
                    每行数据列表.Add(new string[地图宽度]);
                }

                for ( int column, row = 0; row < 地图高度; row++ ) {
                    for ( column = 0; column < 地图宽度; column++ ) {
                        for ( int i = 0; i < 待转化数组列表.Count; i++ ) {
                            每行数据列表[i][column] = 待转化数组列表[i][column, row].ToString();
                        }
                    }

                    for ( int i = 0; i < 待转化数组列表.Count; i++ ) {
                        csv数组列表[i] += string.Join(",", 每行数据列表[i]);
                        if ( row != 地图宽度 - 1 ) csv数组列表[i] += ",";
                        csv数组列表[i] += Environment.NewLine; ;
                    }
                }

                for ( int i = 0; i < 图层名字列表.Count; i++ ) {
                    string layerName = 图层名字列表[i];
                    if ( !世界地图_TMX.Layers.Contains(layerName) ) {
                        MessageBox.Show(string.Format("模板不包含层<{0}>,请重新选择地图模板!", layerName));
                        return false;
                    }
                    else {
                        世界地图_TMX.Layers[layerName].xData.Value = csv数组列表[i];
                    }
                }

            }
            else if ( encoding == "base64" ) {


                List<MemoryStream> 每层地图数据读取流 = new List<MemoryStream>();
                for ( int i = 0; i < 待转化数组列表.Count; i++ ) {
                    每层地图数据读取流.Add(new MemoryStream());
                }

                for ( int column, row = 0; row < 地图高度; row++ ) {
                    for ( column = 0; column < 地图宽度; column++ ) {

                        for ( int i = 0; i < 待转化数组列表.Count; i++ ) {

                            var arr = BitConverter.GetBytes(待转化数组列表[i][column, row]);
                            每层地图数据读取流[i].Write(arr, 0, arr.Length);
                        }
                    }
                }

                for ( int i = 0; i < 待转化数组列表.Count; i++ ) {

                    byte[] buffer = 每层地图数据读取流[i].ToArray();
                    MemoryStream responseStream = new MemoryStream();
                    using ( GZipStream compressedStream = new GZipStream(responseStream, CompressionMode.Compress, true) ) {
                        compressedStream.Write(buffer, 0, buffer.Length);
                    }

                    string text64 = Convert.ToBase64String(responseStream.ToArray());
                    世界地图_TMX.Layers[图层名字列表[i]].xData.Value = text64;
                }
            }
            else throw new Exception("TmxLayer: Unknown encoding.");


            return true;
        }

        public bool TmxIsLoaded() {
            if ( !TmxIsValid() ) {
                return false;
            }

            //if ( 世界地图_TMX == null ) 
            {
                世界地图_TMX = new TmxMap(tb_path.Text);

                地图宽度 = 世界地图_TMX.Width;
                地图高度 = 世界地图_TMX.Height;

                var tilesets = 世界地图_TMX.Tilesets;
                var tsxName = "大地图-标记";
                if ( !tilesets.Contains(tsxName) ) {

                    MessageBox.Show(string.Format("地图文件不包含'{0}'图集,请先添加图集!", tsxName));
                    return false;
                }
                大地图标记起始索引 = tilesets[tsxName].FirstGid;

                tsxName = "随机地图标记";
                if ( !tilesets.Contains(tsxName) ) {

                    MessageBox.Show(string.Format("地图文件不包含'{0}'图集,请先添加图集!", tsxName));
                    return false;
                }
                随机地图标记起始索引 = tilesets[tsxName].FirstGid;
            }

            return true;
        }

        public bool TmxIsValid() {
            string tmxPath = tb_path.Text;
            if ( tmxPath.EndsWith(".tmx") ) {
                if ( !File.Exists(tmxPath) ) {
                    MessageBox.Show("地图文件不存在,请重新选择!");
                    return false;
                }
            }
            else {
                MessageBox.Show("不是有效的地图文件,请重新选择!");
                return false;
            }

            return true;
        }

        public static bool IsFileOpen( string filePath ) {
            bool result = false;
            FileStream fs = null;
            try {
                fs = File.OpenWrite(filePath);
                fs.Close();
            }
            catch ( Exception ) {
                result = true;
            }
            return result;//true 打开 false 没有打开
        }
        /// <summary>
        /// 获取某一层gid值
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="gid"></param>
        /// <returns></returns>
        public bool GetLayerGid(string layerName, int x, int y, ref int gid) {
            if ( !世界地图_TMX.Layers.Contains(layerName) ) {
                MessageBox.Show(string.Format("模板不包含图层<{0}>,请重新选择地图模板!", layerName));
                return false;
            }

            gid = 世界地图_TMX.Layers[layerName].Tiles[x + y * 世界地图_TMX.Width].Gid;

            return true;
        }
        
        /// <summary>
        /// 获取gid对应的属性
        /// </summary>
        /// <param name="gid"></param>
        /// <param name="propertyDict"></param>
        /// <returns></returns>
        public bool GetTileProperty( int gid, ref PropertyDict propertyDict ) {
            if ( gid != 0 ) {
                foreach ( var tileset in 世界地图_TMX.Tilesets ) {

                    int index = gid - tileset.FirstGid;

                    if ( tileset.Tiles.ContainsKey(index) ) {

                        propertyDict = tileset.Tiles[index].Properties;
                        return true;
                    }
                    if ( index < tileset.TileCount ) {

                        MessageBox.Show(string.Format("Gid:{0}对应的图集\"{1}\"没有设置tsx图块属性!", gid, tileset.Name));
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 获取某点某层的某个属性值
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="typeName"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public string GetPropertyByPos( string layerName, string typeName, Point pos ) {

            int gid = 0;
            if ( GetLayerGid(layerName, pos.X, pos.Y, ref gid) ) {

                PropertyDict propertyDict = null;
                if ( GetTileProperty(gid, ref propertyDict) ) {

                    if ( propertyDict.ContainsKey(typeName) ) {
                        return propertyDict[typeName];
                    }

                }
            }
            return string.Empty;
        }
        bool IsInMap( Point pos ) {
            return IsInMap(pos.X, pos.Y);
        }

        bool IsInMap( int x, int y ) {
            if ( x < 地图宽度 && y < 地图高度 &&
                x >= 0 && y >= 0 ) {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 收集全图城市点
        /// </summary>
        /// <param name="cityList"></param>
        bool GetCityPointListInAllMap(ref List<Point> cityList) {
            string layerName = "Layer3";

            for ( int y = 0; y < 地图高度; y++ ) {
                for ( int x = 0; x < 地图宽度; x++ ) {
                    int gid = 0;
                    if ( !GetLayerGid(layerName, x, y, ref gid) ) {
                        return false;
                    }
                    if ( gid != 0 ) {
                        cityList.Add(new Point(x, y));
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 获取区域内的城市点
        /// </summary>
        /// <returns></returns>
        bool GetCityPosInArea( List<Point> areaList , ref List<Point> cityList ) {
            string layerName = "Layer3";

            foreach ( var pos in areaList ) {
                int x = pos.X;
                int y = pos.Y;

                int gid = 0;
                if ( !GetLayerGid(layerName, x, y, ref gid) ) {
                    return false;
                }
                if ( gid != 0 ) {
                    cityList.Add(new Point(x, y));
                }
            }

            return true;
        }

        /// <summary>
        /// 获取同一区域城市的"TerrainType"属性为特定值的城市点列表
        /// </summary>
        /// <param name="areaList"></param>
        /// <param name="propertyValue"></param>
        /// <param name="cityList"></param>
        /// <returns></returns>
        bool GetCityPosListInAreaByPropertyValue( List<Point> areaList, string propertyValue, ref List<Point> cityList ) {
            string layerName = "Layer3";
            string typeName = "TerrainType";

            foreach ( var pos in areaList ) {
                int x = pos.X;
                int y = pos.Y;

                int gid = 0;
                if ( !GetLayerGid(layerName, x, y, ref gid) ) {
                    return false;
                }
                if ( gid != 0 ) {
                    PropertyDict propertyDict = null;
                    if ( GetTileProperty(gid, ref propertyDict) ) {

                        if ( propertyDict.ContainsKey(typeName) && propertyDict[typeName] == propertyValue ) {
                            cityList.Add(new Point(x, y));
                        }

                    }
                }
            }

            return true;

        }

        /// <summary>
        /// 获取每个坐标的某层的所有gid
        /// </summary>
        /// <param name="point_colorGid"></param>
        /// <returns></returns>
        bool SetGidOfLayer( string layerName, ref int[,] point_colorGid ) {
            //string layerName = "Area";

            for ( int y = 0; y < 地图高度; y++ ) {
                for ( int x = 0; x < 地图宽度; x++ ) {
                    int gid = 0;
                    if ( !GetLayerGid(layerName, x, y, ref gid) ) {
                        return false;
                    }

                    point_colorGid[x, y] = gid;
                }
            }
            return true;
        }
        /// <summary>
        /// 递归收集城市所有区域点
        /// </summary>
        /// <param name="cityPosList"></param>
        /// <param name="point_colorGid"></param>
        /// <param name="cityPos"></param>
        /// <param name="pos"></param>
        void CollectCityPosByColor( ref List<Point> cityPosList, int[,] point_colorGid, int cityColorGid, Point pos ) {

            if( !IsInMap(pos) ||
                findleap[pos.X, pos.Y] == 1 ||
                point_colorGid[pos.X, pos.Y] != cityColorGid
                ) {

                return;
            }

            findleap[pos.X, pos.Y] = 1;
            cityPosList.Add(pos);

            foreach ( var offset in 相邻四边偏移量 ) {
                var neighborPos = pos + offset;
                //if ( IsInMap(neighborPos) 
                //    && point_colorGid[neighborPos.X, neighborPos.Y] == cityColorGid
                //    && findleap[neighborPos.X, neighborPos.Y] != 1 ) 
                    {

                    CollectCityPosByColor(ref cityPosList, point_colorGid, cityColorGid, neighborPos);
                }
            }
        }

        /// <summary>
        /// 获取每个城市区域点集合
        /// </summary>
        /// <param name="city_areaList"></param>
        /// <param name="point_colorGid"></param>
        /// <param name="cityList"></param>
        void GetCityAreaPointList( ref Dictionary<Point, List<Point>> city_areaList, string layerName, List<Point> cityList, ref int maxListCount ) {

            int[,] point_colorGid = new int[地图宽度, 地图高度];
            if ( !SetGidOfLayer(layerName, ref point_colorGid) ) {//收集每个郡区域
                return;
            }

            findleap = new int[地图宽度, 地图高度];
            foreach ( var cityPos in cityList ) {
                Log(string.Format("城市{0}!", cityPos));

                List<Point> cityPosList = new List<Point>();
                city_areaList[cityPos] = cityPosList;
                int cityColorGid = point_colorGid[cityPos.X, cityPos.Y];

                CollectCityPosByColor(ref cityPosList, point_colorGid, cityColorGid, cityPos);

                if ( maxListCount < cityPosList.Count ) {

                    maxListCount = cityPosList.Count;
                }
            }

        }
        /// <summary>
        /// 获取某层所有区域点列表
        /// </summary>
        /// <param name="每个郡区域点列表"></param>
        /// <param name="layerName"></param>
        void GetPointListForEachArea(ref List<List<Point>> 每个郡区域点列表 , string layerName) {
            int[,] point_LayerGid = new int[地图宽度, 地图高度];
            if ( !SetGidOfLayer(layerName, ref point_LayerGid) ) {//收集每个郡区域
                return;
            }
            
            findleap = new int[地图宽度, 地图高度];
            for ( int row = 0; row < 地图高度; row++ ) {
                for ( int col = 0; col < 地图宽度; col++ ) {

                    if ( findleap[col, row] != 1 ) {

                        List<Point> 郡区域点列表 = new List<Point>();
                        每个郡区域点列表.Add(郡区域点列表);
                        Point startPos = new Point(col, row);
                        int cityColorGid = point_LayerGid[col, row];

                        CollectCityPosByColor(ref 郡区域点列表, point_LayerGid, cityColorGid, startPos);
                    }
                }
            }
        }

        /// <summary>
        /// 获取某层所有区域点列表, 每块区域颜色不同,不使用递归收集
        /// </summary>
        /// <param name="gid_洲区域点列表"></param>
        /// <param name="layerName"></param>
        void GetGetPointListForEachAreaWhichNotSameColor( ref Dictionary<int, List<Point>> gid_洲区域点列表, string layerName ) {
            int[,] point_LayerGid = new int[地图宽度, 地图高度];
            if ( !SetGidOfLayer(layerName, ref point_LayerGid) ) {//收集每个郡区域
                return;
            }

            for ( int row = 0; row < 地图高度; row++ ) {
                for ( int col = 0; col < 地图宽度; col++ ) {

                    int colorGid = point_LayerGid[col, row];
                    if ( !gid_洲区域点列表.ContainsKey(colorGid) ) {
                        gid_洲区域点列表[colorGid] = new List<Point>();
                    }

                    gid_洲区域点列表[colorGid].Add(new Point(col, row));
                }
            }

        }

        /// <summary>
        /// 该位置是否有遮罩
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        bool IsBlankInMask( int x, int y ) {
            return 遮罩[x, y] == 0;
        }
        bool IsBlankInMask( Point pos ) {
            return IsBlankInMask(pos.X, pos.Y);
        }

        bool IsWhiteMask( int x, int y ) {
            return 遮罩[x, y] == white + 随机地图标记起始索引;
        }
        bool IsWhiteMask( Point pos ) {
            return IsWhiteMask(pos.X, pos.Y);
        }

        bool IsLakeInMask( int x, int y ) {
            return 遮罩[x, y] == 地型_湖泊 + 随机地图标记起始索引;
        }

        bool IsLakeInMask( Point pos ) {
            return IsLakeInMask(pos.X, pos.Y);
        }

        bool IsMountInMask( int x, int y ) {
            return 遮罩[x, y] == 山体 + 随机地图标记起始索引;
        }

        bool IsMountInMask( Point pos ) {
            return IsMountInMask(pos.X, pos.Y);
        }

        bool IsForestInMask( int x, int y ) {
            return 遮罩[x, y] == 树林 + 随机地图标记起始索引;
        }

        bool IsForestInMask( Point pos ) {
            return IsForestInMask(pos.X, pos.Y);
        }

        bool IsBlackInMask( int x, int y ) {
            return 遮罩[x, y] == black +  随机地图标记起始索引;
        }

        bool IsBlackInMask( Point pos ) {
            return IsBlackInMask(pos.X, pos.Y);
        }

        bool IsLake( Point pos ) {
            return IsLake(pos.X, pos.Y);
        }

        bool IsLake( int x, int y ) {
            return 地型地图[x, y] == 地型_湖泊;
        }

        bool IsGrass( Point pos ) {
            return IsGrass(pos.X, pos.Y);
        }

        bool IsGrass( int x, int y ) {
            return 地型地图[x, y] == 地型_草地;
        }

        bool IsForest( Point pos ) {
            return IsForest(pos.X, pos.Y);
        }

        bool IsForest( int x, int y ) {
            return Layer2映射地图[x, y] == 树林;
        }

        bool IsMount( Point pos ) {
            return IsMount(pos.X, pos.Y);
        }

        bool IsMount( int x, int y ) {
            return Layer2映射地图[x, y] == 山体;
        }

        bool HasNotArticle( Point pos ) {
            return HasNotArticle(pos.X, pos.Y);
        }

        bool HasNotArticle( int x, int y ) {
            return Layer2映射地图[x, y] == 0;
        }

        public delegate bool 该位置匹配的地型特征( int x, int y );
        public bool 当前坐标N格范围包含特定地型( int x, int y, int n, 该位置匹配的地型特征 该位置有某地型 ) {
            for ( int row = y - n; row <= y + n; row++ ) {
                for ( int col = x - n; col <= x + n; col++ ) {
                    if ( IsInMap(col, row) && 该位置有某地型(col, row) ) {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool 当前坐标N格范围包含特定地型( Point pos, int n, 该位置匹配的地型特征 该位置有某地型 ) {
            return 当前坐标N格范围包含特定地型( pos.X, pos.Y, n, 该位置有某地型);
        }

        public bool 当前坐标N格范围内没有特定地型( Point pos, int n, 该位置匹配的地型特征 该位置有某地型 ) {
            return 当前坐标N格范围内没有特定地型( pos.X, pos.Y, n, 该位置有某地型);
        }

        public bool 当前坐标N格范围内没有特定地型( int x, int y, int n, 该位置匹配的地型特征 该位置有某地型 ) {
            for ( int row = y - n; row <= y + n; row++ ) {
                for ( int col = x - n; col <= x + n; col++ ) {
                    if ( IsInMap(col, row) && 该位置有某地型(col, row) ) {
                        return false;
                    }
                }
            }
            return true;
        }

        int TotalPos() {
            return 地图宽度 * 地图高度;
        }

        public delegate void 设置地型( Point pos );
        public delegate bool 该位置符合要求( Point pos );

        int 地型格子数 = 0;
        void 地型随机延伸( Point pos, 该位置符合要求 该位置符合要求, 设置地型 设置地型 ) {
            if ( 地型格子数 == 0 ) {
                return;
            }

            设置地型(pos);

            地型格子数--;

            List<Point> 可生成地型列表 = new List<Point>();
            Point nextPos;
            foreach ( var item in 相邻八边偏移量 ) {

                nextPos = pos + item;
                if ( IsInMap(nextPos) ) {

                    可生成地型列表.Add(nextPos);
                }
            }

            while ( 可生成地型列表.Count != 0 ) {
                var pos2 = 可生成地型列表.Random();

                if ( 该位置符合要求(pos2) ) {

                    地型随机延伸(pos2, 该位置符合要求, 设置地型);
                }
                可生成地型列表.Remove(pos2);
            }

        }

        #endregion

        #region 根据城市土地等级生成区域土地等级
        private void btn_level_Click( object sender, EventArgs e ) {

            string savePath = OpenSaveFileDialog("Tiled地图(*.tmx)|*.tmx|所有文件|*.*", "WorldMap_Landlevel", "tmx");
            if( savePath == null ) {
                return;
            }

            if ( !TmxIsLoaded() ) {
                return;
            }

            //全图所有城市点
            List<Point> cityList = new List<Point>();
            if ( !GetCityPointListInAllMap(ref cityList) ) {
                return;
            }
            if ( cityList.Count == 0 ) {
                MessageBox.Show("城市个数为0!");
                return;
            }

            //收集城市点对应的区域点列表
            Dictionary<Point, List<Point>> city_areaList = new Dictionary<Point, List<Point>>();
            int maxListCount = 0;
            GetCityAreaPointList(ref city_areaList, "Area", cityList, ref maxListCount);

            int[,] 土地等级概率生成表 = new int[,] {
                {60, 30, 10, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00,},
                {20, 50, 20, 10, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00,},
                {10, 20, 40, 20, 10, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00,},
                {00, 10, 20, 40, 20, 10, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00,},
                {00, 00, 10, 20, 40, 20, 10, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00,},
                {00, 00, 00, 10, 20, 40, 20, 10, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00,},
                {00, 00, 00, 00, 10, 20, 40, 20, 10, 00, 00, 00, 00, 00, 00, 00, 00, 00,},
                {00, 00, 00, 00, 00, 10, 20, 40, 20, 10, 00, 00, 00, 00, 00, 00, 00, 00,},
                {00, 00, 00, 00, 00, 00, 10, 20, 40, 20, 10, 00, 00, 00, 00, 00, 00, 00,},
                {00, 00, 00, 00, 00, 00, 00, 10, 20, 40, 20, 10, 00, 00, 00, 00, 00, 00,},
                {00, 00, 00, 00, 00, 00, 00, 00, 10, 20, 40, 20, 10, 00, 00, 00, 00, 00,},
                {00, 00, 00, 00, 00, 00, 00, 00, 00, 10, 20, 40, 20, 10, 00, 00, 00, 00,},
                {00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 10, 20, 40, 20, 10, 00, 00, 00,},
                {00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 10, 20, 40, 20, 10, 00, 00,},
                {00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 10, 20, 40, 20, 10, 00,},
                {00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 10, 20, 40, 20, 10,},
                {00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 10, 20, 50, 20,},
                {00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 10, 30, 60,},
            };

            int 城市最大等级 = 18;
            int 土地等级起始索引 = 0;
            int[,] 土地等级 = new int[地图宽度, 地图高度];

            //随机土地等级
            foreach ( var item in city_areaList ) {

                var cityPos = item.Key;
                var landLevel = GetPropertyByPos("LandLevel", "level", cityPos);
                //if ( landLevel == string.Empty ) continue;
                int 城市等级索引 = int.Parse(landLevel) - 1;

                foreach ( var 当前遍历坐标 in item.Value ) {

                    int x = 当前遍历坐标.X;
                    int y = 当前遍历坐标.Y;

                    int random = Range(1, 100);
                    int sum = 0;

                    for ( int Lv = 0; Lv < 城市最大等级; Lv++ ) {

                        sum += 土地等级概率生成表[城市等级索引, Lv];
                        if ( random <= sum ) {
                            土地等级[x, y] = 土地等级起始索引 + Lv;
                            break;
                        }
                    }
                }
            }

            //修复城市的土地等级
            foreach ( var cityPos in city_areaList.Keys ) {
                var matrix = GetPropertyByPos("Layer3", "matrix", cityPos);
                //if ( matrix == string.Empty ) continue;
                int width, height;
                var list = matrix.Split('-');
                if ( list.Length >= 2 && int.TryParse(list[0], out width) && int.TryParse(list[1], out height) ) {

                    Rectangle rect = new Rectangle(cityPos.X, cityPos.Y - height + 1, width, height);
                    //if ( cityPos == new Point(31,281)) {
                    //    Log("X:" + rect.X);
                    //    Log("Y:" + rect.Y);
                    //    Log("Top:" + rect.Top);
                    //    Log("Bottom:" + rect.Bottom);
                    //    Log("Left:" + rect.Left);
                    //    Log("Right:" + rect.Right);
                    //}

                    var landLevel = GetPropertyByPos("LandLevel", "level", cityPos);
                    //if ( landLevel == string.Empty ) continue;
                    int 城市等级索引 = int.Parse(landLevel) - 1;
                    for ( int row = rect.Top; row < rect.Bottom; row++ ) {
                        for ( int col = rect.Left; col < rect.Right; col++ ) {

                            土地等级[col, row] = 土地等级起始索引 + 城市等级索引;
                        }
                    }
                }
            }

            //保存随机结果到tml
            for ( int column = 0, row = 0; row < 地图高度; row++ ) {
                for ( column = 0; column < 地图宽度; column++ ) {

                    土地等级[column, row] +=  大地图标记起始索引;
                }
            }

            List<int[,]> 待转化数组列表 = new List<int[,]> { 土地等级, };
            List<string> 图层名字列表 = new List<string>() { "LandLevel", };

            if ( !SaveDataForEachLayer(待转化数组列表, 图层名字列表) ) {
                return;
            }

            SaveTmx(savePath);
        }
        #endregion

        #region 城市行政区域划分
        private void btn_segment_Click( object sender, EventArgs e ) {

            string savePath = OpenSaveFileDialog("Tiled地图(*.tmx)|*.tmx|所有文件|*.*", "WorldMap_Test", "tmx");
            if ( savePath == null ) {
                return;
            }         

            if ( !TmxIsLoaded() ) {
                return;
            }

            Stopwatch sw = new Stopwatch();
            sw.Reset();sw.Start();

            #region 新规则
            List<List<Point>> 每个郡区域点列表 = new List<List<Point>>();
            GetPointListForEachArea(ref 每个郡区域点列表, "Area1");
            
            #endregion


            List<Point> cityList = new List<Point>();
            if ( !GetCityPointListInAllMap(ref cityList) ) {
                return;
            }

            if ( cityList.Count == 0 ) {
                MessageBox.Show("城市个数为0!");
                return;
            }

            sw.Stop();
            Log(string.Format("收集城市点... 耗时:{0}毫秒!", sw.ElapsedMilliseconds));


            sw.Reset();sw.Start();
            Log(string.Format("城市个数:{0}!", cityList.Count));
            //根据城市个数初始化数据
            Dictionary<Point, List<Point>> city_areaList = new Dictionary<Point, List<Point>>();//每个城市所有的区域点
            //Dictionary<Point, Point> point_city = new Dictionary<Point, Point>();//每个坐标点对应的城市
            Point[,] point_city = new Point[地图宽度, 地图高度];//每个坐标点对应的城市
            Dictionary<Point, List<Point>> city_neighborCityList = new Dictionary<Point, List<Point>>();//每个城市的相邻城市列表  
            foreach ( var cityPos in cityList ) {
                Log(string.Format("城市{0}!", cityPos));
                city_areaList[cityPos] = new List<Point>() { cityPos };
                point_city[cityPos.X, cityPos.Y] = cityPos;
                city_neighborCityList[cityPos] = new List<Point>();
            }
            sw.Stop();
            Log(string.Format("根据城市个数初始化数据...耗时:{0}毫秒!", sw.ElapsedMilliseconds));

            sw.Reset();sw.Start();
            //坐标归属最近的城市

            foreach ( var 郡区域点列表 in 每个郡区域点列表 ) {
                List<Point> 郡区域内城市点列表 = new List<Point>();
                if ( !GetCityPosInArea(郡区域点列表, ref 郡区域内城市点列表) ) {
                    return;
                }
                if ( 郡区域内城市点列表.Count == 0 ) {
                    continue;
                }

                foreach ( var 郡区域点 in 郡区域点列表 ) {

                    //城市本身已经添加跳过
                    if ( 郡区域内城市点列表.Contains(郡区域点) ) {
                        continue;
                    }

                    Point shortestCity = 郡区域内城市点列表[0];
                    int minDis = Distance(郡区域点, shortestCity);

                    for ( int i = 1; i < 郡区域内城市点列表.Count; i++ ) {

                        int dis = Distance(郡区域点, 郡区域内城市点列表[i]);
                        if ( minDis > dis ) {
                            minDis = dis;
                            shortestCity = 郡区域内城市点列表[i];
                        }
                    }

                    city_areaList[shortestCity].Add(郡区域点);
                    point_city[郡区域点.X, 郡区域点.Y] = shortestCity;
                }
            }

            //城市边界不规则化

            //for ( int y = 0; y < 地图高度; y++ ) {
            //    for ( int x = 0; x < 地图宽度; x++ ) {

            //        Point currentPos = new Point(x, y);
            //        if ( IsAtBorderAndNotCitySelf(point_city, currentPos) ) {

            //            int n = 1;
            //            if ( Range(1, 100) < 15 && NumberOfCity(point_city, currentPos, n + 1) == 2 ) {

            //                ReSetPointCityArea(ref city_areaList, ref point_city, currentPos, n);
            //            }
            //        }
            //    }
            //}
            sw.Stop();
            Log(string.Format("坐标归属最近的城市...耗时:{0}毫秒!", sw.ElapsedMilliseconds));

            sw.Reset(); sw.Start();
            //修复bug,重设被分离的区域
            RepairCityArea(ref city_areaList, ref point_city);

            sw.Stop();
            Log(string.Format("寻找修复分离的区域...耗时:{0}毫秒!", sw.ElapsedMilliseconds));

            sw.Reset();sw.Start();
            //记录城市相邻的城市  
            for ( int y = 0; y < 地图高度; y++ ) {
                for ( int x = 0; x < 地图宽度; x++ ) {
                    Point currentPos = new Point(x, y);
                    var cityPos1 = point_city[x, y];

                    foreach ( var offset in 相邻四边偏移量 ) {
                        var neighborPos = currentPos + offset;
                        if ( !IsInMap(neighborPos) ) {
                            continue;
                        }
                        var cityPos2 = point_city[neighborPos.X, neighborPos.Y];
                        //两点不在同一个区域且没有互相设置邻居城市
                        if ( cityPos1 != cityPos2 && !city_neighborCityList[cityPos1].Contains(cityPos2) ) {

                            if ( !city_neighborCityList.ContainsKey(cityPos2) ) {
                                throw new Exception(string.Format("坐标{0}异常!!!!!!!!!!!!!", neighborPos));
                            }
                            city_neighborCityList[cityPos1].Add(cityPos2);
                            city_neighborCityList[cityPos2].Add(cityPos1);
                        }
                    }

                }
            }
            sw.Stop();
            Log(string.Format("记录城市相邻的城市...耗时:{0}毫秒!", sw.ElapsedMilliseconds));

            sw.Reset();sw.Start();
            //城市区上色
            List<int> four_color = new List<int>() { brown, green, blue, purple, white, red, yellow };
            Dictionary<Point, int> city_color = new Dictionary<Point, int>();//每个城市使用区域颜色;
            //city_color[cityList[0]] = white;

            int[,] 区域地图 = new int[地图宽度, 地图高度];
            foreach ( var item in city_areaList ) {
                var posList = item.Value;
                var cityPos = item.Key;
                List<int> 城市可用颜色 = new List<int>(four_color);

                //去掉周围城市已经用掉的颜色
                foreach ( var neighborCityPos in city_neighborCityList[cityPos] ) {
                    if( city_color.ContainsKey(neighborCityPos) ) {
                        城市可用颜色.Remove(city_color[neighborCityPos]);
                    }
                }

                if( 城市可用颜色.Count > 0 ) {
                    city_color[cityPos] = 城市可用颜色[0];
                }
                else {
                    Log(string.Format("城市{0}没有可用的颜色!", cityPos));
                    continue;
                }

                //区域上色
                int color = city_color[cityPos];
                foreach ( var pos in posList ) {
                    区域地图[pos.X, pos.Y] = color;
                }
            }
            sw.Stop();
            Log(string.Format("城市区上色...耗时:{0}毫秒!", sw.ElapsedMilliseconds));

            sw.Reset();sw.Start();

            int[,] Area = new int[地图宽度, 地图高度];

            for ( int column = 0, row = 0; row < 地图高度; row++ ) {
                for ( column = 0; column < 地图宽度; column++ ) {
                    if ( 区域地图[column, row] != 0 ) {
                        Area[column, row] = 区域地图[column, row] + 随机地图标记起始索引;
                    }
                    else {
                        Area[column, row] = 0;
                    }
                }
            }

            List<int[,]> 待转化数组列表 = new List<int[,]> { Area, };
            List<string> 图层名字列表 = new List<string>() { "Area", };

            if( !SaveDataForEachLayer(待转化数组列表, 图层名字列表) ) {
                return;
            }

            sw.Stop();
            Log(string.Format("tmx转化...耗时:{0}毫秒!", sw.ElapsedMilliseconds));

            sw.Reset();sw.Start();
            SaveTmx(savePath);
            sw.Stop();
            Log(string.Format("保存...耗时:{0}毫秒!", sw.ElapsedMilliseconds));

            //MessageBox.Show("区域划分完成");
        }

        /// <summary>
        /// 中心N格范围内有无城市
        /// </summary>
        /// <param name="point_city"></param>
        /// <param name="pos"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        bool HaveCityInDistaneN( Point[,] point_city, Point pos, int n) {
            int x = pos.X;
            int y = pos.Y;
            for ( int row = y - n; row <= y + n; row++ ) {
                for ( int col = x - n; col <= x + n; col++ ) {
                    Point currentPos = new Point(col, row);
                    if ( IsInMap(currentPos) && point_city[col, row] == currentPos ) {
                        return true;
                    }
                }
            }
            return false;
        }

        //坐标是否是在城市边界且一格范围内没有城市本身
        bool IsAtBorderAndNotCitySelf( Point[,] point_city, Point pos ) {
            if( HaveCityInDistaneN(point_city, pos, 1) ) {
                return false;
            }

            foreach ( var offset in 相邻八边偏移量 ) {
                var currentPos = pos + offset;
                if( IsInMap(currentPos) && point_city[pos.X, pos.Y] != point_city[currentPos.X, currentPos.Y] ) {
                    return true;
                }
            }
            return false;
        }
        //区域N个范围内城市区域的个数
        int NumberOfCity( Point[,] point_city, Point pos, int n ) {
            List<Point> cityList = new List<Point>();
            int x = pos.X;
            int y = pos.Y;

            for ( int row = y - n; row <= y + n; row++ ) {
                for ( int col = x - n; col <= x + n; col++ ) {

                    Point currentPos = new Point(col, row);
                    if( IsInMap(currentPos) && !cityList.Contains(point_city[currentPos.X, currentPos.Y]) ) {
                        cityList.Add(point_city[currentPos.X, currentPos.Y]);
                    }
                }
            }

            return cityList.Count;
        }

        //城市边界区域轮廓重置
        void ReSetPointCityArea( ref Dictionary<Point, List<Point>> city_areaList, ref Point[,] point_city, Point pos, int n ) {
            int x = pos.X;
            int y = pos.Y;
            var currentCity = point_city[pos.X, pos.Y];

            for ( int row = y - n; row <= y + n; row++ ) {
                for ( int col = x - n; col <= x + n; col++ ) {

                    Point neighborPos = new Point(col, row);

                    if ( IsInMap(neighborPos) && currentCity != point_city[neighborPos.X, neighborPos.Y] ) {

                        var neighborCity = point_city[neighborPos.X, neighborPos.Y];
                        city_areaList[neighborCity].Remove(neighborPos);

                        city_areaList[currentCity].Add(neighborPos);
                        point_city[neighborPos.X, neighborPos.Y] = currentCity;

                    }
                }
            }
        }
        /// <summary>
        /// 修复被分离的区域
        /// </summary>
        int[,] findleap;
        void RepairCityArea( ref Dictionary<Point, List<Point>> city_areaList, ref Point[,] point_city ) {
            Dictionary<Point, List<Point>> city_areaList_compare = new Dictionary<Point, List<Point>>();
            findleap = new int[地图宽度, 地图高度];

            foreach ( var cityPos in city_areaList ) {

                List<Point> cityList = new List<Point>();
                city_areaList_compare[cityPos.Key] = cityList;

                CollectCityPos(ref cityList, point_city, cityPos.Key, cityPos.Key);
            }

            //输出异常坐标
            foreach ( var cityPos in city_areaList.Keys ) {
                if ( city_areaList[cityPos].Count != city_areaList_compare[cityPos].Count ) {
                    Log(string.Format("城市{0}缺失区域!!!!", cityPos));

                    foreach ( var pos in city_areaList[cityPos] ) {
                        if ( !city_areaList_compare[cityPos].Contains(pos) ) {
                            Log(string.Format("坐标{0}被分离", pos));
                        }
                    }
                }
            }

            for ( int col = 0, row = 0; row < 地图高度; row++ ) {
                for ( col = 0; col < 地图宽度; col++ ) {
                    if ( findleap[col, row] != 1 ) {
                        //Log(string.Format("{0}-{1}缺失", col, row));

                        var currentCityPos = point_city[col, row];
                        Point currentPos = new Point(col, row);
                        foreach ( var offset in 相邻四边偏移量 ) {
                            var neighborPos = currentPos + offset;

                            if( IsInMap(neighborPos) && point_city[neighborPos.X, neighborPos.Y] != currentCityPos ) {

                                findleap[col, row] = 1;
                                var neighborCityPos = point_city[neighborPos.X, neighborPos.Y];

                                city_areaList[currentCityPos].Remove(currentPos);
                                city_areaList[neighborCityPos].Add(currentPos);
                                point_city[col, row] = neighborCityPos;
                                break;
                            }

                        }
                    }
                }
            }
            for ( int col = 0, row = 0; row < 地图高度; row++ ) {
                for ( col = 0; col < 地图宽度; col++ ) {
                    if ( findleap[col, row] != 1 ) {
                        Log(string.Format("{0}-{1}缺失", col, row));
                    }
                }
            }
        }
        /// <summary>
        /// 递归收集城市区域(不包括分离的区域)
        /// </summary>
        /// <param name="point_city"></param>
        /// <param name="cityPos"></param>
        /// <param name="pos"></param>
        void CollectCityPos( ref List<Point> cityList, Point[,] point_city, Point cityPos, Point pos ) {

            findleap[pos.X, pos.Y] = 1;
            cityList.Add(pos);

            foreach ( var offset in 相邻四边偏移量 ) {
                var neighborPos = pos + offset;
                if ( IsInMap(neighborPos) && point_city[neighborPos.X, neighborPos.Y] == cityPos && findleap[neighborPos.X, neighborPos.Y] != 1 ) {

                    CollectCityPos(ref cityList, point_city, cityPos, neighborPos);
                }
            }
        }
        #endregion

        #region 导出城市区域
        private void btn_export_area_Click( object sender, EventArgs e ) {
            if ( !TmxIsLoaded() ) {
                return;
            }

            string savePath = OpenSaveFileDialog("Excel表格(*.xlsx)|*.xlsx|所有文件|*.*", "WorldMap_PosData", "xlsx", true);
            if ( savePath == null ) {
                return;
            }

            //全图所有城市点
            List<Point> cityList = new List<Point>();
            if ( !GetCityPointListInAllMap(ref cityList) ) {
                return;
            }
            if ( cityList.Count == 0 ) {
                MessageBox.Show("城市个数为0!");
                return;
            }

            //设置每个点对应的郡府
            List<List<Point>> 每个郡区域点列表 = new List<List<Point>>();
            GetPointListForEachArea(ref 每个郡区域点列表, "Area1");

            Point[,] point_郡首府 = new Point[地图宽度, 地图高度];
            foreach ( var 郡区域点列表 in 每个郡区域点列表 ) {
                List<Point> 选出城市点列表 = new List<Point>();
                GetCityPosListInAreaByPropertyValue(郡区域点列表, Area1_ID.Text, ref 选出城市点列表);
                List<Point> 郡洲合一城市点列表 = new List<Point>();
                GetCityPosListInAreaByPropertyValue(郡区域点列表, Area_1_2_ID.Text, ref 郡洲合一城市点列表);
                
                Point 郡首府位置;
                if ( 郡洲合一城市点列表.Count == 1 ) {

                    郡首府位置 = 郡洲合一城市点列表[0];
                }
                else if ( 郡洲合一城市点列表.Count >= 2 ) {

                    throw new Exception(string.Format("坐标{0},{1}多个郡首府异常!!!!!!!!!!!!!", 郡洲合一城市点列表[0], 郡洲合一城市点列表[1]));
                }
                else if ( 选出城市点列表.Count == 1 ) {

                    郡首府位置 = 选出城市点列表[0];
                }
                else if ( 选出城市点列表.Count >= 2 ) {

                    throw new Exception(string.Format("坐标{0},{1}多个郡首府异常!!!!!!!!!!!!!", 选出城市点列表[0], 选出城市点列表[1]));
                }
                else {
                    throw new Exception(string.Format("坐标{0}区域没有郡首府异常!!!!!!!!!!!!!", 郡区域点列表[0]));
                    //郡首府位置 = 郡区域点列表[0];
                }

                foreach ( var pos in 郡区域点列表 ) {
                    point_郡首府[pos.X, pos.Y] = 郡首府位置;
                }
            }

            //设置每个点对应的洲府
            Dictionary<int, List<Point>> gid_洲区域点列表 = new Dictionary<int, List<Point>>();
            GetGetPointListForEachAreaWhichNotSameColor(ref gid_洲区域点列表, "Area2");
            //List<List<Point>> 每个洲区域点列表 = new List<List<Point>>();
            //GetPointListForEachArea(ref 每个洲区域点列表, "Area2");

            Point[,] point_洲首府 = new Point[地图宽度, 地图高度];
            foreach ( var 洲区域点列表 in gid_洲区域点列表.Values ) {
                List<Point> 选出城市点列表 = new List<Point>();
                GetCityPosListInAreaByPropertyValue(洲区域点列表, Area2_ID.Text, ref 选出城市点列表);
                List<Point> 郡洲合一城市点列表 = new List<Point>();
                GetCityPosListInAreaByPropertyValue(洲区域点列表, Area_1_2_ID.Text, ref 郡洲合一城市点列表);

                Point 洲首府位置;
                if ( 郡洲合一城市点列表.Count == 1 ) {

                    洲首府位置 = 郡洲合一城市点列表[0];
                }
                else if ( 郡洲合一城市点列表.Count >= 2 ) {

                    throw new Exception(string.Format("坐标{0},{1}多个洲首府异常!!!!!!!!!!!!!", 郡洲合一城市点列表[0], 郡洲合一城市点列表[1]));
                }
                else if ( 选出城市点列表.Count == 1 ) {

                    洲首府位置 = 选出城市点列表[0];
                }
                else if ( 选出城市点列表.Count >= 2 ) {

                    throw new Exception(string.Format("坐标{0},{1}多个洲首府异常!!!!!!!!!!!!!", 选出城市点列表[0], 选出城市点列表[1]));
                }
                else {
                    throw new Exception(string.Format("坐标{0}区域没有洲郡首府异常!!!!!!!!!!!!!", 洲区域点列表[0]));
                    //洲首府位置 = 洲区域点列表[0];
                }

                foreach ( var pos in 洲区域点列表 ) {
                    point_洲首府[pos.X, pos.Y] = 洲首府位置;
                }
            }


            //收集城市点对应的区域点列表
            Dictionary<Point, List<Point>> city_areaList = new Dictionary<Point, List<Point>>();
            int maxListCount = 0;
            GetCityAreaPointList(ref city_areaList, "Area", cityList, ref maxListCount);


            Point[,] point_县首府位置 = new Point[地图宽度, 地图高度];
            string[,] point_TerrainType = new string[地图宽度, 地图高度];

            foreach ( var item in city_areaList ) {
                var cityPos = item.Key;
                string layerName = "Layer3";
                string typeName = "TerrainType";
                string terrainType = GetPropertyByPos(layerName, typeName, cityPos);

                foreach ( var pos in item.Value ) {
                    point_县首府位置[pos.X, pos.Y] = cityPos;
                    point_TerrainType[pos.X, pos.Y] = terrainType;
                }
            }

            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet1 = workbook.CreateSheet("城市区域");
            
            for ( int i = 0; i < cityList.Count; i++ ) {
                //row.CreateCell(i).SetCellValue(cityList[i].ToString());
                sheet1.SetColumnWidth(i, 256 * 12);
            }

            int count = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 地图高度 * 地图宽度;

            for ( int y = 0; y < 地图高度; y++ ) {
                for ( int x = 0; x < 地图宽度; x++ ) {

                    IRow row = sheet1.CreateRow(count);
                    var 县首府位置 = point_县首府位置[x, y];
                    var 郡首府位置 = point_郡首府[x, y];
                    var 洲首府位置 = point_洲首府[x, y];

                    row.CreateCell(0).SetCellValue(string.Format("{0}_{1}", x, y));
                    row.CreateCell(1).SetCellValue(string.Format("{0}_{1}", 县首府位置.X, 县首府位置.Y));
                    row.CreateCell(2).SetCellValue(point_TerrainType[x, y]);

                    row.CreateCell(3).SetCellValue(string.Format("{0}_{1}", 郡首府位置.X, 郡首府位置.Y));
                    row.CreateCell(4).SetCellValue(string.Format("{0}_{1}", 洲首府位置.X, 洲首府位置.Y));


                    count++;
                    progressBar1.Value = count;
                }
            }

            //for ( int y = 0; y < maxListCount; y++ ) {
            //    IRow row = sheet1.CreateRow(y);
            //    for ( int x = 0; x < cityList.Count; x++ ) {
            //        var cityPos = cityList[x];
            //        if ( city_areaList[cityPos].Count > y ) {
            //            var currentPos = city_areaList[cityPos][y];
            //            row.CreateCell(x).SetCellValue(string.Format("{0}-{1}", currentPos.X, currentPos.Y));

            //            progressBar1.Value = ( count > progressBar1.Maximum ? progressBar1.Maximum : count );
            //            count++;
            //        }
            //    }
            //}

            FileStream sw = File.Create(savePath);

            workbook.Write(sw);

            sw.Close();
            MessageBox.Show("导出完成!");
        }
        #endregion
        
        private void Form1_Load( object sender, EventArgs e ) {
            湖泊label2.Text = "湖泊:" + 湖泊trackBar1.Value.ToString() + "%";
            湖泊概率 = 湖泊trackBar1.Value / 100.0f;

            山体label3.Text = "山体:" + 山体trackBar2.Value.ToString() + "%";
            山体概率 = 山体trackBar2.Value / 100.0f;

            树林label4.Text = "树林:" + 树林trackBar3.Value.ToString() + "%";
            树林概率 = 树林trackBar3.Value / 100.0f;

            湖泊装饰label5.Text = "湖泊装饰:" + 湖泊装饰trackBar4.Value.ToString() + "%";
            湖泊装饰物概率 = 湖泊装饰trackBar4.Value / 100.0f;

            陆地装饰label2.Text = "陆地装饰:" + 陆地trackBar1.Value.ToString() + "%";
            陆地装饰物概率 = 陆地trackBar1.Value / 100.0f;
        }

        private void trackBar1_Scroll( object sender, EventArgs e ) {
            湖泊label2.Text = "湖泊:" + 湖泊trackBar1.Value.ToString() + "%";
            湖泊概率 = 湖泊trackBar1.Value / 100.0f;
        }

        private void trackBar2_Scroll( object sender, EventArgs e ) {
            山体label3.Text = "山体:" + 山体trackBar2.Value.ToString() + "%";
            山体概率 = 山体trackBar2.Value / 100.0f;
        }

        private void trackBar3_Scroll( object sender, EventArgs e ) {
            树林label4.Text = "树林:" + 树林trackBar3.Value.ToString() + "%";
            树林概率 = 树林trackBar3.Value / 100.0f;
        }

        private void trackBar4_Scroll( object sender, EventArgs e ) {

            湖泊装饰label5.Text = "湖泊装饰:" + 湖泊装饰trackBar4.Value.ToString() + "%";
            湖泊装饰物概率 = 湖泊装饰trackBar4.Value / 100.0f;
        }

        private void 陆地trackBar1_Scroll( object sender, EventArgs e ) {

            陆地装饰label2.Text = "陆地装饰:" + 陆地trackBar1.Value.ToString() + "%";
            陆地装饰物概率 = 陆地trackBar1.Value / 100.0f;
        }
    }
}


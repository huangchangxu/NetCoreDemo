using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Pens;
using SixLabors.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace PaymentCenter.Infrastructure.Tools
{
    /// <summary>
    /// 验证码生成类，不能继续该类 
    /// </summary>
    public class ValidateCodeTool
    {
        /// <summary>  
        /// 该方法用于生成指定位数的随机数  
        /// </summary>  
        /// <param name="VcodeNum">参数是随机数的位数</param>  
        /// <returns>返回一个随机数字符串</returns>  
        private static string RndNum(int VcodeNum)
        {
            //验证码可以显示的字符集合  
            string Vchar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,p" +
                ",q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,P,P,Q" +
                ",R,S,T,U,V,W,X,Y,Z";
            string[] VcArray = Vchar.Split(new Char[] { ',' });//拆分成数组   
            string code = "";//产生的随机数  
            int temp = -1;//记录上次随机数值，尽量避避免生产几个一样的随机数  

            Random rand = new Random();
            //采用一个简单的算法以保证生成随机数的不同  
            for (int i = 1; i < VcodeNum + 1; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));//初始化随机类  
                }
                int t = rand.Next(61);//获取随机数  
                if (temp != -1 && temp == t)
                {
                    return RndNum(VcodeNum);//如果获取的随机数重复，则递归调用  
                }
                temp = t;//把本次产生的随机数记录起来  
                code += VcArray[t];//随机数的位数加一  
            }
            return code;
        }
        /// <summary>
        /// 画点+画字=验证码byte[]
        /// </summary>
        /// <param name="code"></param>
        /// <param name="vCodeNum"></param>
        /// <param name="xx"></param>
        /// <param name="yy"></param>
        /// <returns></returns>
        public static byte[] GetValidCodeByte(string fontPath,out string code, int vCodeNum=4,int xx = 80, int yy = 25)
        {
            var bb = default(byte[]);
            code = RndNum(vCodeNum);
            try
            {
                var content = code;
                var dianWith = 1; //点宽度
                var xx_space = 10;  //点与点之间x坐标间隔
                var yy_space = 5;    //y坐标间隔
                var wenZiLen = vCodeNum;  //文字长度
                var maxX = xx / wenZiLen; //每个文字最大x宽度
                var prevWenZiX = 0; //前面一个文字的x坐标
                var size = maxX;//字体大小

                //字体
                var install_Family = new FontCollection().Install(fontPath);
                //var install_Family = new FontCollection().Find("arial");
                var font = new Font(install_Family, size);  //字体

                //点坐标
                var listPath = new List<IPath>();
                for (int i = 0; i < xx / xx_space; i++)
                {
                    for (int j = 0; j < yy / yy_space; j++)
                    {
                        var position = new Vector2(i * xx_space, j * yy_space);
                        var linerLine = new LinearLineSegment(position, position);
                        var shapesPath = new SixLabors.Shapes.Path(linerLine);
                        listPath.Add(shapesPath);
                    }
                }

                //画图
                using (Image<Rgba32> image = new Image<Rgba32>(xx, yy))   //画布大小
                {
                    image.Mutate(x =>
                    {
                        var imgProc = x;

                        //逐个画字
                        for (int i = 0; i < wenZiLen; i++)
                        {
                            //当前的要输出的字
                            var nowWenZi = content.Substring(i, 1);

                            //文字坐标
                            var wenXY = new Vector2();
                            var maxXX = prevWenZiX + (maxX - size);
                            wenXY.X = new Random().Next(prevWenZiX, maxXX);
                            wenXY.Y = new Random().Next(0, yy - size);

                            prevWenZiX = Convert.ToInt32(Math.Floor(wenXY.X)) + size;

                            //画字
                            imgProc.DrawText(
                                   nowWenZi,   //文字内容
                                   font,
                                   i % 2 > 0 ? Rgba32.HotPink : Rgba32.Red,
                                   wenXY,
                                   TextGraphicsOptions.Default);
                        }

                        //画点 
                        imgProc.BackgroundColor(Rgba32.WhiteSmoke).   //画布背景
                                     Draw(
                                     Pens.Dot(Rgba32.HotPink, dianWith),   //大小
                                     new PathCollection(listPath)  //坐标集合
                                 );
                    });
                    using (MemoryStream stream = new MemoryStream())
                    {
                        image.SaveAsPng(stream);
                        bb = stream.GetBuffer();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return bb;
        }

    }
}

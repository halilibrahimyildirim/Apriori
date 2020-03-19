namespace BIL3003_Odev1
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.Start = new System.Windows.Forms.Button();
            this.support = new System.Windows.Forms.TextBox();
            this.measureValue = new System.Windows.Forms.TextBox();
            this.measures = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.FreqItemSets = new System.Windows.Forms.ListBox();
            this.BinRanges = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Rules = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.RulesCount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Start
            // 
            this.Start.Location = new System.Drawing.Point(139, 64);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(75, 23);
            this.Start.TabIndex = 0;
            this.Start.Text = "Execute";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // support
            // 
            this.support.Location = new System.Drawing.Point(139, 14);
            this.support.Name = "support";
            this.support.Size = new System.Drawing.Size(100, 20);
            this.support.TabIndex = 1;
            // 
            // measureValue
            // 
            this.measureValue.Location = new System.Drawing.Point(139, 38);
            this.measureValue.Name = "measureValue";
            this.measureValue.Size = new System.Drawing.Size(100, 20);
            this.measureValue.TabIndex = 2;
            // 
            // measures
            // 
            this.measures.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.measures.FormattingEnabled = true;
            this.measures.Items.AddRange(new object[] {
            "Confidince",
            "Lift",
            "Leverage"});
            this.measures.Location = new System.Drawing.Point(11, 38);
            this.measures.Name = "measures";
            this.measures.Size = new System.Drawing.Size(121, 21);
            this.measures.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(8, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 18);
            this.label1.TabIndex = 4;
            this.label1.Text = "Minimum Support";
            // 
            // FreqItemSets
            // 
            this.FreqItemSets.FormattingEnabled = true;
            this.FreqItemSets.HorizontalScrollbar = true;
            this.FreqItemSets.Location = new System.Drawing.Point(257, 30);
            this.FreqItemSets.Name = "FreqItemSets";
            this.FreqItemSets.Size = new System.Drawing.Size(291, 173);
            this.FreqItemSets.TabIndex = 5;
            // 
            // BinRanges
            // 
            this.BinRanges.FormattingEnabled = true;
            this.BinRanges.Location = new System.Drawing.Point(15, 108);
            this.BinRanges.Name = "BinRanges";
            this.BinRanges.Size = new System.Drawing.Size(149, 95);
            this.BinRanges.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(254, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(333, 18);
            this.label2.TabIndex = 7;
            this.label2.Text = "Frequent Item Sets and Minimum Support Counts";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label3.Location = new System.Drawing.Point(12, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 18);
            this.label3.TabIndex = 8;
            this.label3.Text = "Bin Ranges";
            // 
            // Rules
            // 
            this.Rules.FormattingEnabled = true;
            this.Rules.HorizontalScrollbar = true;
            this.Rules.Location = new System.Drawing.Point(15, 253);
            this.Rules.Name = "Rules";
            this.Rules.Size = new System.Drawing.Size(562, 277);
            this.Rules.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.Location = new System.Drawing.Point(12, 232);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 18);
            this.label4.TabIndex = 10;
            this.label4.Text = "Rules";
            // 
            // RulesCount
            // 
            this.RulesCount.AutoSize = true;
            this.RulesCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.RulesCount.Location = new System.Drawing.Point(310, 232);
            this.RulesCount.Name = "RulesCount";
            this.RulesCount.Size = new System.Drawing.Size(107, 18);
            this.RulesCount.TabIndex = 11;
            this.RulesCount.Text = "Rules Count = ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 537);
            this.Controls.Add(this.RulesCount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Rules);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BinRanges);
            this.Controls.Add(this.FreqItemSets);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.measures);
            this.Controls.Add(this.measureValue);
            this.Controls.Add(this.support);
            this.Controls.Add(this.Start);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Data Miner";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.TextBox support;
        private System.Windows.Forms.TextBox measureValue;
        private System.Windows.Forms.ComboBox measures;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox FreqItemSets;
        private System.Windows.Forms.ListBox BinRanges;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox Rules;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label RulesCount;
    }
}


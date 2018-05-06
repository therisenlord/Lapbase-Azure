<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
    <html>
    <body>
		<table style="width:100%" cellpadding = "0" cellspacing = "0" border="0" >
			<tr>
				<td>
					<table>
						<tr>
							<td>Date</td>
							<td>Weight</td>
							<td>%EWL</td>
						</tr>
					</table>
				</td>
				<td>
					<table>
						<tr>
							<td rowspan="2">
								Hyper-<br/>tension
							</td>
							<td colspan="4">Lipids</td>
							<td colspan="3">Diabetes</td>
						</tr>
						<tr>
							<td></td>
							<td>Chol.</td>
							<td>T.GL.</td>
							<td>HDLC.</td>

							<td></td>
							<td>F.Gluc.</td>
							<td>HBA1C</td>
						</tr>
					</table>
				</td>
				<td>
					<table>
						<tr>
							<td>Asthma</td>
							<td>Reflux</td>
							<td>Sleep</td>
							<td>Fert</td>
							<td>Incont.</td>
							<td>Black</td>
							<td>Joint</td>
							<td>Cardio.</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<br/>
		<table style="width:100%" cellpadding = "0" cellspacing = "0" border="0" id ="tblDoctorXSLT" class="testNameTable">
			<xsl:for-each select ="dsSchema/tblComorbidities">
			<tr>
				<td>
					<table>
						<tr>
							<td>
								<xsl:value-of select="tempDateSeen"/>
							</td>
							<td>
								<xsl:value-of select="round(Weight)"/>
							</td>
							<td>
								<xsl:value-of select="round(EWL)"/>
							</td>
						</tr>
					</table>
				</td>
				<td>
					<table>
						<tr>
							<td>
								<input type="checkbox">
									<xsl:attribute name ="id">chkBaseHypertensionProblems_<xsl:value-of select="position()"/></xsl:attribute>
									<xsl:attribute name ="checked"><xsl:value-of select="BaseHypertensionProblems"/></xsl:attribute>
								</input>
							</td>
							<td>
								<xsl:value-of select="SystolicBP"/>/<xsl:value-of select="DiastolicBP"/>
							</td>
							<td>
								<input type="checkbox">
									<xsl:attribute name ="id">chkBaseLipidProblems_<xsl:value-of select="position()"/></xsl:attribute>
									<xsl:attribute name ="checked"><xsl:value-of select="BaseLipidProblems"/></xsl:attribute>
								</input>
							</td>
							<td>
								<xsl:value-of select="TotalCholesterol"/>
							</td>
							<td>
								<xsl:value-of select="Triglycerides"/>
							</td>
							<td>
								<xsl:value-of select="HDLCholesterol"/>
							</td>

							<td>
								<input type="checkbox">
									<xsl:attribute name="id">chkBaseDiabetesProblems_<xsl:value-of select="position()"/></xsl:attribute>
									<xsl:attribute name="checked"><xsl:value-of select="BaseDiabetesProblems"/></xsl:attribute>
								</input>
							</td>
							<td>
								<xsl:value-of select="FBloodGlucose"/>
							</td>
							<td>
								<xsl:value-of select="HBA1C"/>
							</td>
						</tr>
					</table>
				</td>
				<td>
					<table>
						<tr>
							<td>
								<xsl:value-of select="AsthmaCurrentLevel"/>
							</td>
							<td>
								<xsl:value-of select="RefluxCurrentLevel"/>
							</td>
							<td>
								<xsl:value-of select="SleepCurrentLevel"/>
							</td>
							<td>
								<xsl:value-of select="FertilityCurrentLevel"/>
							</td>
							<td>
								<xsl:value-of select="IncontinenceCurrentLevel"/>
							</td>
							<td>
								<xsl:value-of select="BackCurrentLevel"/>
							</td>
							<td>
								<xsl:value-of select="ArthritisCurrentLevel"/>
							</td>
							<td>
								<xsl:value-of select="CVDLevelCurrentLevel"/>
							</td>
						</tr>
					</table>
				</td>
			</tr>
			</xsl:for-each>
		</table>
    </body>
    </html>
</xsl:template>

</xsl:stylesheet> 


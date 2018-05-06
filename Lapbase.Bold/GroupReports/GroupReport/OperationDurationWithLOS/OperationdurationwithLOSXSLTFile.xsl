<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:xs="http://www.w3.org/2001/XMLSchema"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt" >
<xsl:key name="keySurgeryType" match="/dsSchema/tblApproach" use="SurgeryType"/>
	<!-- <xsl:for-each select="/dsSchema/tblCategory">
	<xsl:if test="($SurgeryType = SurgeryType) and ($Approach = Approach)">
		-->
	<xsl:key name="keySurgeryType_Approach" match="/dsSchema/tblCategory" use="concat(SurgeryType,Approach)"/>
<xsl:template match="/">
<html>
	<head>
		<link href="http://www.lapbase.net/app_css/v2/admin_common.css" rel="stylesheet" type="text/css"/>
	</head>
	<body class="printPreview">
		<div class="report4">
			<div class="clr"></div>
			<table class="details" border="0">
				<tr>
					<td style="width:89px">
						<strong>Surgeon:</strong>
					</td>
					<td style="width:376px">
						<xsl:value-of select='translate(dsSchema/tblOperations/DoctorName_Title, "`", "&apos;")'/>
					</td>
					<td rowspan="4" width="313">
						<h2>Operation duration with LOS</h2>
						<p class="printDate">
							<xsl:value-of select='dsSchema/tblOperations/ReportDate'/>
						</p>
					</td>
				</tr>
				<tr>
					<td>
						<strong>Hospital:</strong>
					</td>
					<td>
						<xsl:value-of select="dsSchema/tblOperations/HospitalName_Title"/>
					</td>
				</tr>
				<tr>
					<td>
						<strong>Surgery:</strong>
					</td>
					<td>
						<!--<xsl:value-of select="dsSchema/tblOperations/SurgeryType_Desc"/>-->
					</td>
				</tr>
				<tr>
					<td>&#160;</td>
					<td>&#160;</td>
				</tr>
			</table>

			<table class="data" border="0" style="width:797px">
				<xsl:for-each select="/dsSchema/tblSurgery">
					<xsl:variable name ="SurgeryType" select="SurgeryType"/>
					<xsl:variable name ="cntSurgery" select="count(/dsSchema/tblOperations[SurgeryType = $SurgeryType])"/>
					<xsl:variable name ="sumDuration" select="sum(/dsSchema/tblOperations[SurgeryType = $SurgeryType]/Duration)"/>
					<xsl:variable name ="sumStay" select="sum(/dsSchema/tblOperations[SurgeryType = $SurgeryType]/Stay)"/>
				<tr>
					<td colspan="2" class="surgeryType"></td>
					<td class="surgeryType" style="width:20px"></td>
					<td class="surgeryType" style="width:25px"></td>
					<td class="surgeryType" style="width:40px"></td>
					<td class="surgeryType" style="width:117px">
						<div align="center">Duration (mins)</div>
					</td>
					<td class="surgeryType" style="width:116px">
						<div align="center">Stay (days)</div>
					</td>
				</tr>
				<tr>
					<td colspan="2" class="surgeryType">
						<strong>
							<xsl:value-of select="SurgeryType_Desc"/>
						</strong>
					</td>
					<td class="surgeryType" style="width:20px">
						<span>
							<xsl:value-of select="$cntSurgery"/>
						</span>
					</td>
					<td class="surgeryType" style="width:25px">&#160;</td>
					<td class="surgeryType" style="width:40px">&#160;</td>
					<td class="surgeryType" style="width:117px">
						<div align="center">
							<strong>
								<xsl:value-of select="round($sumDuration div $cntSurgery)"/>
							</strong>
						</div>
					</td>
					<td class="surgeryType" style="width:116px">
						<div align="center">
							<strong>
								<xsl:value-of select="round($sumStay div $cntSurgery)"/>
							</strong>
						</div>
					</td>
				</tr>
					<xsl:for-each select="key('keySurgeryType', $SurgeryType)">
						<!--<xsl:if test="$SurgeryType = SurgeryType">-->
							<xsl:variable name="Approach" select="Approach"/>
							<xsl:variable name ="cntApproach" select="count(/dsSchema/tblOperations[(SurgeryType = $SurgeryType) and (Approach = $Approach)])"/>
							<xsl:variable name ="sumApproach_Duration" select="sum(/dsSchema/tblOperations[(SurgeryType = $SurgeryType) and (Approach = $Approach)]/Duration)"/>
							<xsl:variable name ="sumApproach_Stay" select="sum(/dsSchema/tblOperations[(SurgeryType = $SurgeryType) and (Approach = $Approach)]/Stay)"/>
						<tr>
							<td style="width:15px">&#160;</td>
							<td style="width:434px">
								<strong>
									<xsl:value-of select="Approach"/>
								</strong>
							</td>
							<td>&#160;</td>
							<td>
								<span>
									<xsl:value-of select="$cntApproach"/>
								</span>
							</td>
							<td>&#160;</td>
							<td>
								<div align="center">
									<strong>
										<xsl:value-of select="round($sumApproach_Duration div $cntApproach)"/>
									</strong>
								</div>
							</td>
							<td>
								<div align="center">
									<strong><xsl:value-of select="round($sumApproach_Stay div $cntApproach)"/></strong>
								</div>
							</td>
						</tr>
						<!--<xsl:key name="keySurgeryType_Approach" match="/dsSchema/tblCategory" use="concat(SurgeryType,Approach)"/>-->
						<xsl:for-each select="key('keySurgeryType_Approach', concat(SurgeryType,Approach))">
						<!--
							<xsl:for-each select="/dsSchema/tblCategory">
								<xsl:if test="($SurgeryType = SurgeryType) and ($Approach = Approach)">-->
									<xsl:variable name="Category" select="Category"/>
									<xsl:variable name ="cntCategory" select="count(/dsSchema/tblOperations[(SurgeryType = $SurgeryType) and (Approach = $Approach) and (Category = $Category)])"/>
									<xsl:variable name ="sumCategory_Duration" select="sum(/dsSchema/tblOperations[(SurgeryType = $SurgeryType) and (Approach = $Approach) and (Category = $Category)]/Duration)"/>
									<xsl:variable name ="sumCategory_Stay" select="sum(/dsSchema/tblOperations[(SurgeryType = $SurgeryType) and (Approach = $Approach) and (Category = $Category)]/Stay)"/>
								<tr>
									<td>&#160;</td>
									<td>
										<xsl:value-of select="Category_Desc"/>
									</td>
									<td>&#160;</td>
									<td>&#160;</td>
									<td>
										<span>
											<xsl:value-of select="$cntCategory"/>
										</span>
									</td>
									<td>
										<div align="center">
											<xsl:value-of select="round($sumCategory_Duration div $cntCategory)"/>
										</div>
									</td>
									<td>
										<div align="center">
											<xsl:value-of select="round($sumCategory_Stay div $cntCategory)"/>
										</div>
									</td>
								</tr>
								<!--</xsl:if>-->
							</xsl:for-each>
						<!--</xsl:if>-->
					</xsl:for-each>
				</xsl:for-each>
			</table>
			<img src="../../img/banner_sml.gif" class="smlBanner"/>
		</div>
	</body>
</html>
</xsl:template>

</xsl:stylesheet> 


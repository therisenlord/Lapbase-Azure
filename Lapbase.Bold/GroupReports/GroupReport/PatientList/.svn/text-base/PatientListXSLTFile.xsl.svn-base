<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:xs="http://www.w3.org/2001/XMLSchema"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt" >

<xsl:template match="/">
<html>
	<head>
		<link href="http://www.lapbase.net/app_css/v2/admin_common.css" rel="stylesheet" type="text/css"/>
	</head>
    <body class="printPreview">
		<div class="report6">
			<img src="../../img/print_header_bar.gif" height="8" width="800px" />
			<div class="clr"></div>
			<table class="details" border="0">
				<tr>
					<td style="width:89px">
						<strong>Surgeon:</strong>
					</td>
					<td style="width:376px">
						<xsl:value-of select='dsSchema/tblPatientList/DoctorName_Title'/>
					</td>
					<td rowspan="4" width="313">
						<h2>Patient Details</h2>
						<p class="printDate">
							<xsl:value-of select='dsSchema/tblPatientList/ReportDate'/>
						</p>
					</td>
				</tr>
				<tr>
					<td>
						<strong>Hospital:</strong>
					</td>
					<td>
						<xsl:value-of select='dsSchema/tblPatientList/HospitalName_Title'/>
					</td>
				</tr>
				<tr>
					<td><strong>Surgery:</strong></td>
					<td></td>
				</tr>
				<tr>
					<td>&#160;</td>
					<td>&#160;</td>
				</tr>
			</table>
			<xsl:variable select ="/dsSchema/tblPatientList/WeightMeasurment" name ="WeightMeasurment"/>
			<table class="data" border="0" style="width:1020px">
				<tr style="text-align:center; vertical-align:bottom">
					<th colspan="5" align="left">&#160;</th>
					<th colspan="3" class="blueHighlight" style="text-align:left; vertical-align:bottom" >Surgery</th>
					<th rowspan="2" style="width:36px">Lost to FU</th>
					<th rowspan="2" style="width:36px">Inital BMI</th>
          <th rowspan="2" style="width:36px">
            Initial Weight (<xsl:value-of select="$WeightMeasurment"/>)
          </th>
					<th rowspan="2" style="width:36px">
						Ideal Weight (<xsl:value-of select="$WeightMeasurment"/>)
					</th>
					<th rowspan="2" style="width:37px">
						Target Weight (<xsl:value-of select="$WeightMeasurment"/>)
					</th>
					<th rowspan="2" style="width:37px">
						Current Weight (<xsl:value-of select="$WeightMeasurment"/>)
					</th>
					<th rowspan="2" style="width:37px">Current BMI</th>
					<th class="highlight" style="text-align:left; vertical-align:bottom" >&#160;</th>
					<th colspan="2" class="highlight" style="text-align:left; vertical-align:bottom" >Last Visit</th>
				</tr>
				<tr style="text-align:left">
					<th colspan="2" >Patient Name</th>
					<th style="width:159px">Address</th>
					<th style="width:18px">Age</th>
					<th style="width:21px">Sex</th>
					<th class="blueHighlight" style="width:71px">Date</th>
					<th class="blueHighlight" style="width:28px">Months</th>
					<th class="highlight" style="width:135px">&#160;</th>
					<th class="highlight" style="width:10px">&#160;</th>
					<th class="highlight" style="width:42px">Mths</th>
					<th class="highlight" style="width:126px">Date</th>
				</tr>
				<xsl:for-each select="/dsSchema/tblPatientList">
					<tr>
						<td style="width:22px">
							<strong>
								<xsl:value-of select="PatientID"/>
							</strong>
						</td>
						<td style="width:99px">
							<xsl:value-of select="PatientName"/>
						</td>
						<td>
							<xsl:value-of select="Address"/>
						</td>
						<td>
							<xsl:value-of select="AGE"/>
						</td>
						<td>
							<xsl:value-of select="Sex"/>
						</td>
						<td class="blueHighlight">
							<xsl:value-of select="OperationDate"/>
						</td>
						<td class="blueHighlight">
							<xsl:value-of select="MthsSinceOperation"/>
						</td>
						<td class="blueHighlight">
							<xsl:value-of select="SurgeryType_Desc"/>
						</td>
						<td align="center">&#9744;</td>
						<td align="center">
							<xsl:value-of select="round(InitBMI)"/>
						</td>
						<td align="center">
							<xsl:value-of select="round(StartWeight)"/>
						</td>
						<td align="center">
							<xsl:value-of select="round(IdealWeight)"/>
						</td>
						<td align="center">
							<xsl:value-of select="round(TargetWeight)"/>
						</td>
						<td align="center">
							<xsl:value-of select="round(CurrentWeight)"/>
						</td>
						<td align="center" class="highlight">
							<xsl:value-of select="round(CurrentBMI)"/>
						</td>
						<td class="highlight">
							
						</td>
						<td class="highlight">
							<xsl:value-of select="MthsSinceVisit"/>
						</td>
						<td class="highlight">
							<xsl:value-of select="DateSeen"/>
						</td>
					</tr>
				</xsl:for-each>
			</table>
		</div>
		<img src="../../img/banner_sml.gif" class="smlBanner"/>
	</body>
</html>
</xsl:template>

</xsl:stylesheet> 


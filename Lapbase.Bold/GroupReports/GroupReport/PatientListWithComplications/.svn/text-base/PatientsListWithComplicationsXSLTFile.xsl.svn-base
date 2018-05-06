<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:xs="http://www.w3.org/2001/XMLSchema"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt" >

<xsl:key name="keyPatientId" match="/dsSchema/tblPatientList" use="PatientId"/>
<xsl:template match="/">
<html>
	<head>
		<link href="http://www.lapbase.net/app_css/v2/admin_common.css" rel="stylesheet" type="text/css"/>
	</head>
	<body class="printPreview">
		<div class="report5">
			<img src="../../img/print_header_bar.gif" height="8" width="800px" />
			<div class="clr"></div>

			<table class="details" border="0">
				<tbody>
					<tr>
						<td width="89">
							<strong>Surgeon:</strong>
						</td>
						<td width="376">
							<xsl:value-of select='dsSchema/tblPatientList/DoctorName_Title'/>
						</td>
						<td rowspan="4" width="313">
							<h2>Patient List including Operation, Weight loss and Complications</h2>
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
						<td>
							<strong>Surgery:</strong>
						</td>
						<td></td>
					</tr>
					<tr>
						<td>&#160;</td>
						<td>&#160;</td>
					</tr>
				</tbody>
			</table>

			<table class="data" border="0" width="800">
				<tbody>
					<tr style="vertical-align:top">
						<th style="width:84px">
							<strong>Op. Date</strong>
						</th>
						<th style="width:53px">
							<strong>Patient</strong>
						</th>
						<th style="width:160px">
							<strong>Surgery</strong>
						</th>
						<th style="width:76px">
							<strong>Approach</strong>
						</th>
						<th style="width:45px">
							<strong>Inital BMI</strong>
						</th>
						<th style="width:48px">
							<strong>
								Weight Loss (<xsl:value-of select="/dsSchema/tblPatient/WeightMeasurment"/>)
							</strong>
						</th>
						<th style="width:61px">
							<strong>%EWL</strong>
						</th>
						<th >
							<strong>Comlications / Events</strong>
						</th>
					</tr>
					<xsl:for-each select="/dsSchema/tblPatient">
						<tr style="vertical-align:top">
							<td >
								<xsl:value-of select="OperationDate"/></td>
							<td >
								<strong>
									<xsl:value-of select="PatientId"/>
								</strong>
							</td>
							<td >
								<span>
									<xsl:value-of select="SurgeryType_Desc"/>
								</span>
							</td>
							<td >
								<xsl:value-of select="Approach"/>
							</td>
							<td >
								<xsl:if test ="BMI != 0">
									<xsl:value-of select="BMI"/>
								</xsl:if>
							</td>
							<td >
								<span>
									<xsl:if test ="WeightLoss != 0">
										<xsl:value-of select="round(WeightLoss)"/>
									</xsl:if>
								</span>
							</td>
							<td >
								<xsl:if test ="EWL != 0">
									%<xsl:value-of select="round(EWL)"/>
								</xsl:if>
							</td>
							<td style="width:221px; vertical-align:top">
								<xsl:variable name ="PatientId" select="PatientId"/>
								<table border="0" width="100%">
									<!--class="complication" -->
									<xsl:for-each select="key('keyPatientId', $PatientId)">
									<tr style="vertical-align:top">
										<td style="width:35%">
											<xsl:value-of select="ComplicationDate"/>
										</td>
										<td >
											<xsl:value-of select="Complication"/>
										</td>
									</tr>
									</xsl:for-each>
								</table>
							</td>
						</tr>
					</xsl:for-each>
				</tbody>
			</table>

			<img src="../../img/banner_sml.gif" class="smlBanner"/>
		</div>
	</body>
</html>
</xsl:template>

</xsl:stylesheet> 


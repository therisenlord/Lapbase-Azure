<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:xs="http://www.w3.org/2001/XMLSchema"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt" >
	<xsl:key name="keyCompType" match="/dsSchema/tblSurgery" use="CompType"/>
	<xsl:key name="keyComplicationCode_SurgeryType" match="/dsSchema/tblSurgery_Comp" use="concat(ComplicationCode,SurgeryType)"/>
	<xsl:template match="/">
		<html>
			<head>
				<link href="http://www.lapbase.net/app_css/v2/admin_common.css" rel="stylesheet" type="text/css"/>
			</head>
			<body class="printPreview">
				<div class="complicationsReport">
					<img src="../../../img/print_header_bar.gif" height="8" width="800px" />
					<table class ="details" border="0">
						<tr>
							<td style="width:89px">
								<strong>Surgeon:</strong>
							</td>
							<td style="width:376px">
								<xsl:value-of select='translate(dsSchema/tblComplicationSummary/DoctorName_Title, "`", "&apos;")'/>
							</td>
							<td rowspan="4" style="width:313px">
								<h2>- COMPLICATIONS<br/>- READMISSIONS</h2>
								<p class="printDate">
									<xsl:value-of select="dsSchema/tblComplicationSummary/ReportDate"/>
								</p>
							</td>
						</tr>
						<tr>
							<td>
								<strong>Hospital:</strong>
							</td>
							<td>
								<xsl:value-of select="dsSchema/tblComplicationSummary/HospitalName_Title"/>
							</td>
						</tr>

						<tr>
							<td>
								<strong>Surgery:</strong>
							</td>
							<td>
								<!--<xsl:value-of select="dsSchema/tblComplicationSummary/SurgeryType_Desc"/>-->
							</td>
						</tr>

						<tr>
							<td>&#160;</td>
							<td>&#160;</td>
						</tr>
					</table>

					<table border="0" style="width:710px">
						<xsl:for-each select="/dsSchema/tblComplication">
							<xsl:variable name="CompType" select="CompType"/>
							<tr>
								<td colspan="4">
									<h3><xsl:value-of select="$CompType"/></h3>
								</td>
							</tr>
							<xsl:for-each select="key('keyCompType', $CompType)">
								<tr>
									<td colspan="4">
										<h4>
											<xsl:value-of select="SurgeryType_Desc"/>
										</h4>
									</td>
								</tr>
								<xsl:variable name ="ComplicationCode" select="ComplicationCode"/>
								<xsl:variable name ="SurgeryType" select="SurgeryType"/>
								<!--<xsl:key name="keyComplicationCode_SurgeryType" match="/dsSchema/tblSurgery_Comp" use="concat(ComplicationCode,SurgeryType)"/>-->
								<!--
								<xsl:for-each select="/dsSchema/tblSurgery_Comp">
									<xsl:if test ="($ComplicationCode = ComplicationCode) and ($SurgeryType = SurgeryType)">-->
								<xsl:for-each select="key('keyComplicationCode_SurgeryType', concat($ComplicationCode,$SurgeryType))">
										<xsl:variable name ="cntOfComplication" select="count(/dsSchema/tblComplicationSummary[(ComplicationCode = $ComplicationCode) and (SurgeryType = $SurgeryType)]) "/>
										<xsl:variable name ="sumOfComplicationWeeks" select="sum(/dsSchema/tblComplicationSummary[(ComplicationCode = $ComplicationCode) and (SurgeryType = $SurgeryType)]/Weeks) "/>
										<xsl:variable name ="cntReadmitted" select="count(/dsSchema/tblComplicationSummary[(ComplicationCode = $ComplicationCode) and (SurgeryType = $SurgeryType) and (Readmitted = 1)]) "/>
										<tr>
											<td>
												<xsl:value-of select="Complication"/>
											</td>
											<td >
												Number <xsl:value-of select="$cntOfComplication"/>
											</td>
											<td>
												Av weeks post op <xsl:value-of select="round($sumOfComplicationWeeks div $cntOfComplication)"/>
											</td>
										</tr>
										<tr>
											<td colspan="2"/>
											<td>
												Readmissions <xsl:value-of select="$cntReadmitted"/>
											</td>
										</tr>
									<!--</xsl:if>-->
								</xsl:for-each>
							</xsl:for-each>
						</xsl:for-each>
					</table>
					<img src="../../img/banner_sml.gif" class="smlBanner"/>
				</div>
			</body>
		</html>
	</xsl:template>

</xsl:stylesheet> 


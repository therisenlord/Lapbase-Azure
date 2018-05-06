<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt">
<xsl:key name="kVisitDate" match="/dsSchema/tblHistoryData" use="strVisitDate"/>
<xsl:template match="/">
    <html>
		<head>
			<style type="text/css">.style2 {color: #000000}</style>
		</head>
    <body >
		<table border="0" class="inner_table" cellpadding="0" cellspacing="0" >
			<tr >
			<xsl:for-each select="dsSchema/tblVisitDate">
				<!--<xsl:sort select ="strVisitDate" data-type ="text" order ="descending"/>-->
				<xsl:variable name="strVisitDate" select="strVisitDate"/>
				<td style="vertical-align:top">
					<table border="0" cellpadding="0" cellspacing="0" >
						<xsl:choose>
							<xsl:when test="position() mod 2 = 1">
								<xsl:attribute name="bgColor">#ffffff</xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="bgColor">#efefef</xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
						<tr style="height:29px;vertical-align:top" >
							<td >
								<xsl:value-of select ="$strVisitDate"/>
							</td>
						</tr>
						<xsl:for-each select ="key('kVisitDate', $strVisitDate)">
						<tr style="height:29px;vertical-align:top" >
							<td>
								<xsl:value-of select ="Value"/>
							</td>
						</tr>
						</xsl:for-each>
					</table>
				</td>
			</xsl:for-each>
			</tr>
		</table>
    </body>
    </html>
</xsl:template>

</xsl:stylesheet> 


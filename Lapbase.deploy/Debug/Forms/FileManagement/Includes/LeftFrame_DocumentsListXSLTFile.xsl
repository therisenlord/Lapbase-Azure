<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt">

<xsl:template match="/">
  <html>
    <body>
      <xsl:choose>
        <xsl:when test="dsSchema/tblParent/ShowBy = 'TYPE'">
            <xsl:for-each select="dsSchema/tblParent">
			  <xsl:variable name = "TypeID" select="Type_ID"/>
					<xsl:if test ="position() = 1">
						<input type="text" style="display:none" id ="txtFirstType" >
							<xsl:attribute name ="value">
								<xsl:value-of select="Type_ID"/>
							</xsl:attribute>
						</input>
					</xsl:if>
                  <a href="#" >
					  <xsl:attribute name ="onclick">
						  javascript:DocumentTypeCaption_onclick(this, <xsl:value-of select="Type_ID"/>);
					  </xsl:attribute>
					  
					<h4>
						<xsl:choose>
							<xsl:when test ="Type_ID = 1">
								<img runat="server" alt="." src="../../../img/files_ico_picture.gif" border="0"></img>
							</xsl:when>
							<xsl:when test ="Type_ID = 2">
								<img runat="server" alt="." src="../../../img/files_ico_movie.gif" border="0"></img>	
							</xsl:when>
							<xsl:when test ="Type_ID = 3">
								<img runat="server" alt="." src="../../../img/files_ico_doc.gif" border="0"></img>
							</xsl:when>
						</xsl:choose>
						<xsl:value-of select="Type_Name"/> (viewall)
					</h4>
                  </a>
                    <xsl:for-each select ="/dsSchema/tblChild">
                      <xsl:sort select = "Original_EventDate" data-type = "text" order = "descending"/>
                      <xsl:if test="$TypeID = DocumentType">
							  <ul>
								  <li>
									  <a href="#">
										  <xsl:attribute name ="onclick">
											  javascript:tempDIV_onclick(<xsl:value-of select="tblPatientDocumentsID"/>, <xsl:value-of select="IsDeleted"/>, <xsl:value-of select="Type_ID"/>)
										  </xsl:attribute>
										  <xsl:attribute name="title">
											  <xsl:value-of select="Doc_Description"/>
										  </xsl:attribute>
										  <xsl:if test="IsDeleted = 'true'">
											  <xsl:attribute name="style">color:#AAAAAA</xsl:attribute>
										  </xsl:if>
										  <xsl:if test="IsDeleted = 'true'">
											  <span style="font-size:x-small;color:#DD0000;font-weight:bold">X</span>&#160;
										  </xsl:if>
										  <xsl:if test ="DocumentName != ''">
											  <xsl:value-of select = "DocumentName"/>
										  </xsl:if>
										  <xsl:if test ="DocumentName = ''">
											  <xsl:value-of select = "DocumentFileName"/>
										  </xsl:if>
									  </a>
								  </li>
							  </ul>
                      </xsl:if>
                    </xsl:for-each>
            </xsl:for-each>
        </xsl:when>
        <xsl:when test="dsSchema/tblParent/ShowBy = 'TYPEDATE'">
            <xsl:for-each select="dsSchema/tblParent">
              <xsl:variable name = "TypeID" select="Type_ID"/>
                  <a href="#">
                    <xsl:attribute name ="onclick">
                      javascript:LoadDocumentItems(<xsl:value-of select="Type_ID"/>);
                    </xsl:attribute>
					  <h4>
						  <xsl:choose>
							  <xsl:when test ="Type_ID = 1">
								  <img runat="server" alt="." src="../../../img/files_ico_picture.gif" border="0"></img>
							  </xsl:when>
							  <xsl:when test ="Type_ID = 2">
								  <img runat="server" alt="." src="../../../img/files_ico_movie.gif" border="0"></img>
							  </xsl:when>
							  <xsl:when test ="Type_ID = 3">
								  <img runat="server" alt="." src="../../../img/files_ico_doc.gif" border="0"></img>
							  </xsl:when>
						  </xsl:choose>
						  <xsl:value-of select="Type_Name"/> (viewall)
					  </h4>
                  </a>
                    <xsl:for-each select ="/dsSchema/tblChild">
                      <xsl:sort select = "Type_ID" data-type = "number" order = "ascending"/>
                      <xsl:sort select = "Original_EventDate" data-type = "text" order = "descending"/>
                      <xsl:variable name ="idx" select="position()-1"/>
                      <xsl:if test="$TypeID = DocumentType">
						  <ul>
							  <li>
								  <table style="width:100%" cellspacing="0" cellpadding="0" border="0">
									  <tr>
										  <td style="width:30%">
											  <a href="#">
												  <xsl:attribute name ="onclick">
													  javascript:LoadDocumentItems_SpecificDate(<xsl:value-of select="Type_ID"/>, '<xsl:value-of select="EventDate"/>');
												  </xsl:attribute>
												  <!--<xsl:value-of select="EventDate"/>-->

												  <xsl:if test ="IsLoad = 1">
													  <xsl:value-of select="EventDate"/>
												  </xsl:if>
												  <xsl:if test ="IsLoad != 1">
													  <xsl:if test="EventDate != //tblChild[$idx]/EventDate">
														  <xsl:value-of select="EventDate"/>
													  </xsl:if>
												  </xsl:if>
											  </a>
										  </td>
										  <td style="width:15%">
											  <a href="#">
												  <xsl:attribute name ="onclick">
													  javascript:LoadDocumentItems_SpecificEvent(<xsl:value-of select="Type_ID"/>,
													  '<xsl:value-of select="EventDate"/>', '<xsl:value-of select="EventLink"/>');
												  </xsl:attribute>

												  
												  
												  <xsl:if test ="IsLoad = 1">
													  <xsl:value-of select="EventName"/>
												  </xsl:if>

												  <xsl:if test ="IsLoad != 1">

													  <xsl:if test="EventDate != //tblChild[$idx]/EventDate">
														  <xsl:value-of select="EventName"/>
													  </xsl:if>

													  <xsl:if test="(EventDate = //tblChild[$idx]/EventDate) and (EventName != //tblChild[$idx]/EventName)">
														  <xsl:value-of select="EventName"/>
													  </xsl:if>
												  </xsl:if>
											  </a>
										  </td>
										  <td style="width:55%">
											  <a href="#">
												  
												  
												  <xsl:attribute name="title">
													  <xsl:value-of select="Doc_Description"/>
												  </xsl:attribute>
												  <xsl:attribute name ="onclick">
													  javascript:tempDIV_onclick(<xsl:value-of select="tblPatientDocumentsID"/>, <xsl:value-of select="IsDeleted"/>, <xsl:value-of select="Type_ID"/>)
												  </xsl:attribute>
												  <xsl:if test="IsDeleted = 'true'">
													  <xsl:attribute name="style">color:#AAAAAA</xsl:attribute>
												  </xsl:if>
												  <xsl:if test="IsDeleted = 'true'">
													  <span style="font-size:x-small;color:#DD0000;font-weight:bold">X</span>&#160;
												  </xsl:if>
												  
												  <xsl:if test ="DocumentName != ''">
													  <xsl:value-of select = "DocumentName"/>
												  </xsl:if>
												  <xsl:if test ="DocumentName = ''">
													  <xsl:value-of select = "DocumentFileName"/>
												  </xsl:if>
											  </a>
										  </td>
									  </tr>
								  </table>
							  </li>
						  </ul>
                      </xsl:if>
                    </xsl:for-each>
            </xsl:for-each>
        </xsl:when>
        <xsl:when test="dsSchema/tblParent/ShowBy = 'DATE'">
            <xsl:for-each select="dsSchema/tblParent">
              <xsl:sort select = "Original_EventDate" data-type = "text" order = "descending"/>
              <xsl:variable name = "strEventDate" select="EventDate"/>
              <xsl:variable name = "strEventName" select="EventName"/>
				<ui>
					<li>
						<table cellspacing ="0" cellpadding="3" border="0" style="font-size: 0.9em;width:100%">
							<tr>
								<td style="width:30%">
									<a href="#">
										<xsl:attribute name ="onclick">
											javascript:LoadDocumentItems_SpecificDate(0, '<xsl:value-of select="EventDate"/>');
										</xsl:attribute>
										<xsl:value-of select="EventDate"/>
									</a>
								</td>
								<td style="width:15%">
									<xsl:value-of select="EventName"/>
								</td>
								<td style="width:55%">
									<xsl:for-each select ="/dsSchema/tblChild">
										<xsl:if test ="($strEventDate = EventDate) and ($strEventName = EventName)">
											<a href="#" >
												<xsl:attribute name="title">
													<xsl:value-of select="Doc_Description"/>
												</xsl:attribute>
												<xsl:attribute name ="onclick">
													javascript:tempDIV_onclick(<xsl:value-of select="tblPatientDocumentsID"/>, <xsl:value-of select="IsDeleted"/>, <xsl:value-of select="DocumentType"/>)
												</xsl:attribute>
												<xsl:if test="IsDeleted = 'true'">
													<xsl:attribute name="style">color:#AAAAAA</xsl:attribute>
												</xsl:if>
												<xsl:if test="IsDeleted = 'true'">
													<span style="font-size:x-small;color:#DD0000;font-weight:bold">X</span>&#160;
												</xsl:if>
												<xsl:choose>
													<xsl:when test ="DocumentType = 1">
														<img runat="server" alt="." src="../../../img/files_ico_picture.gif" border="0"></img>
													</xsl:when>
													<xsl:when test ="DocumentType = 2">
														<img runat="server" alt="." src="../../../img/files_ico_movie.gif" border="0"></img>
													</xsl:when>
													<xsl:when test ="DocumentType = 3">
														<img runat="server" alt="." src="../../../img/files_ico_doc.gif" border="0"></img>
													</xsl:when>
												</xsl:choose>&#160;
												<xsl:if test ="string-length(DocumentName)!= 0">
													<xsl:value-of select = "DocumentName"/>
												</xsl:if>
												<xsl:if test ="string-length(DocumentName) = 0">
													<xsl:value-of select = "DocumentFileName"/>
												</xsl:if>
											</a>
											<br/>
										</xsl:if>
									</xsl:for-each>
								</td>
							</tr>
						</table>
					</li>
				</ui>
            </xsl:for-each>
        </xsl:when>
      </xsl:choose>
    </body>
  </html>
</xsl:template>

</xsl:stylesheet> 


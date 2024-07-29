"use client";
import { Tabs } from "antd";
import TabPane from "antd/es/tabs/TabPane";
import React, { useEffect, useState } from "react";
import { PicturesTab } from "./(TabItems)/PicturesTab";
import { ReelsTab } from "./(TabItems)/ReelsTab";
import { VideosTab } from "./(TabItems)/VideosTab";
import axios from "axios";

export interface TabData {
  items: {
    generatedCaption: string | null;
    clipEmbedding?: never;
    id: string;
    contentId: string;
    name: string;
    fileExtension: string;
    tags?: string;
    dateUploaded: string;
    isPrivate: boolean;
    owner?: string;
    authorizedRoles: string[];
    originalSource: string | null;
  }[];
  totalCount: number;
}

const Library = () => {
  const [tabData, setTaBData] = useState<TabData>();
  const tabKeys: { [key: number]: string } = {
    1: "https://www.cicki.gratis/api/Image/PaginatedWithCaption",
    2: "https://www.cicki.gratis/api/Image/PaginatedWithCaption",
    3: "https://www.cicki.gratis/api/Image/PaginatedWithCaption",
  };

  useEffect(() => {
    axios
      .get("https://www.cicki.gratis/api/Image/PaginatedWithCaption", {
        params: {
          page: 1,
          pageSize: 10,
        },
        headers: {
          Authorization: localStorage.getItem("token"),
        },
      })
      .then((response) => {
        console.log(response);
        if (response.status !== 200) return;
        setTaBData(response.data);
      })
      .catch((error) => {
        console.log(error);
      });
  }, []);

  const onTabChange = (activeKey: number) => {
    axios
      .get(tabKeys[activeKey], {
        params: {
          page: 1,
          pageSize: 10,
        },
        headers: {
          Authorization: localStorage.getItem("token"),
        },
      })
      .then((response) => {
        console.log(response);
        if (response.status !== 200) return;
        setTaBData(response.data);
      })
      .catch((error) => {
        console.log(error);
      });
  };
  return (
    <div>
      <Tabs
        defaultActiveKey="1"
        onTabClick={(activeKey) => onTabChange(Number.parseInt(activeKey))}
        items={[
          {
            label: `Pictures`,
            key: "1",
            children: <PicturesTab tabData={tabData} />,
          },
          {
            label: `Reels`,
            key: "2",
            children: <ReelsTab tabData={tabData} />,
          },
          {
            label: `Videos`,
            key: "3",
            children: <VideosTab tabData={tabData} />,
          },
        ]}
      >
        <TabPane tab="Pictures" key="1"></TabPane>
        <TabPane tab="Reels" key="2">
          test
        </TabPane>
        <TabPane tab="Videos" key="3">
          test
        </TabPane>
      </Tabs>
    </div>
  );
};

export default Library;

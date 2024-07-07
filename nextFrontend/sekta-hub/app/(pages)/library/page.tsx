import { Tabs } from "antd";
import TabPane from "antd/es/tabs/TabPane";
import React from "react";
import { PicturesTab } from "./(TabItems)/PicturesTab";
import { ReelsTab } from "./(TabItems)/ReelsTab";
import { VideosTab } from "./(TabItems)/VideosTab";

const Library = () => {
  return (
    <div>
      <Tabs
        defaultActiveKey="1"
        items={[
          {
            label: `Pictures`,
            key: "1",
            children: <PicturesTab />,
          },
          {
            label: `Reels`,
            key: "2",
            children: <ReelsTab />,
          },
          {
            label: `Videos`,
            key: "3",
            children: <VideosTab />,
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

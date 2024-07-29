import { Image } from "antd";
import { TabData } from "../page";
import NoImageFound from "../../../assets/csm_no-image_d5c4ab1322.png";
import classNames from "classnames";
import "../styles.css";

interface Props {
  tabData?: TabData;
}
export const PicturesTab = ({ tabData }: Props) => {
  return (
    <div>
      {tabData?.items?.map((item) => {
        const isValidImage = ["jpeg", "png", "jpg"].includes(
          item.fileExtension
        );
        return (
          <span key={item.id} className="images">
            <Image
              className={classNames({ noImageFound: !isValidImage })}
              src={
                isValidImage
                  ? `https://www.cicki.gratis/api/Image/${item.id}/Thumbnail`
                  : NoImageFound.src
              }
              alt="Picture"
              preview={false}
            />
          </span>
        );
      })}
    </div>
  );
};

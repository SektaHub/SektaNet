import { Image } from "antd";

export const PicturesTab = () => {
  const pictures = ["1", "2", "3", "4", "5"];

  return (
    <div>
      {pictures.map((picture) => (
        <Image
          key={picture}
          src={`https://picsum.photos/id/${picture}/200/300`}
          alt="Picture"
        />
      ))}
    </div>
  );
};

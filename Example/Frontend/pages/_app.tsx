import "../styles/globals.css";
import type { AppProps } from "next/app";
import { useEffect } from "react";
import { API } from "../models/api";
import axios from "axios";

// Set Default Axios Parameters
axios.defaults.baseURL = "http://localhost:5017";
axios.defaults.headers.common["Content-Type"] = "application/json";

const MyApp: React.FC<AppProps> = ({ Component, pageProps }) => {
  const getAllUsers = async () => {
    try {
      const users = await API.Users.All.Request();
      users.forEach((u) => {
        console.log(u.email);
      });
    } catch (error) {
      console.log("Error");
      console.log(error);
    }
  };

  const getAllDepartments = async () => {
    try {
      const requestPayload: API.Departments.All.IRequestModel = {
        listAll: true,
        isDeleted: false,
      };
      const departments = await API.Departments.All.Request(requestPayload);
      departments.forEach((u) => {
        console.log(u.name);
      });
    } catch (error) {
      console.log("Error");
      console.log(error);
    }
  };

  useEffect(() => {
    getAllUsers();
    getAllDepartments();

    console.log(API.Users.All.RequestPath);
  }, []);

  return <Component {...pageProps} />;
};

export default MyApp;

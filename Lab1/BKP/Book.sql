--
-- PostgreSQL database dump
--

-- Dumped from database version 15.0
-- Dumped by pg_dump version 15.0

-- Started on 2022-10-31 12:20:29

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 215 (class 1259 OID 16421)
-- Name: Book; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Book" (
    "Id" character varying NOT NULL,
    "Fotograf" character varying,
    "Mladenac" character varying,
    "Mladenka" character varying,
    "Format" character varying,
    "Korice" character varying,
    "Broj listova" integer,
    "Paket" boolean,
    "Faceoff" character varying,
    "Cijena" integer
);


ALTER TABLE public."Book" OWNER TO postgres;

--
-- TOC entry 3321 (class 0 OID 16421)
-- Dependencies: 215
-- Data for Name: Book; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Book" ("Id", "Fotograf", "Mladenac", "Mladenka", "Format", "Korice", "Broj listova", "Paket", "Faceoff", "Cijena") FROM stdin;
1	135	140	300	30x40	bijeli	21	f	bijeli	500
2	139	154	315	30x30	bijeli	32	f	bijeli	550
3	316	141	301	30x40	crni	36	f	crni	600
4	136	142	302	30x40	crni	35	t	bijeli	550
5	136	142	302	20x27	crni	35	t	bijeli	300
6	136	142	302	20x27	crni	35	t	bijeli	300
7	137	145	316	40x40	bež	40	t	bež	800
8	137	145	316	20x20	bež	40	t	bež	400
9	137	145	316	20x20	bež	40	t	bež	400
10	135	146	317	30x30	bijeli	32	f	bijeli	550
11	135	147	318	35x35	crni	32	f	crni	600
12	139	148	319	25x25	bijeli	37	f	crni	550
13	316	150	303	30x30	crni	35	f	crni	600
14	138	151	304	20x20	bež	30	f	crni	350
\.


--
-- TOC entry 3175 (class 2606 OID 16427)
-- Name: Book Data_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Book"
    ADD CONSTRAINT "Data_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 3176 (class 2606 OID 16433)
-- Name: Book Fotograf; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Book"
    ADD CONSTRAINT "Fotograf" FOREIGN KEY ("Fotograf") REFERENCES public."Osobe"("OIB") NOT VALID;


--
-- TOC entry 3177 (class 2606 OID 16438)
-- Name: Book Mladenac; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Book"
    ADD CONSTRAINT "Mladenac" FOREIGN KEY ("Mladenac") REFERENCES public."Osobe"("OIB") NOT VALID;


--
-- TOC entry 3178 (class 2606 OID 16443)
-- Name: Book Mladenka; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Book"
    ADD CONSTRAINT "Mladenka" FOREIGN KEY ("Mladenka") REFERENCES public."Osobe"("OIB") NOT VALID;


-- Completed on 2022-10-31 12:20:29

--
-- PostgreSQL database dump complete
--

